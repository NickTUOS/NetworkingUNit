using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Helper.Net.RUDP;
using Helper.Net.UDT;

namespace Test.Application
{
	public partial class TestingSocketForm : Form
	{
		#region Variables

		Thread thread;
		System.Windows.Forms.Timer timer;
		System.Windows.Forms.Timer timerGraph;

		//----
		bool stopUDP = false;

		//----
		bool IsReceiveMode = false;

		//---- Sockets
		private RUDPSocket rudpSocket;
		private RUDPSocket newRUDPSocket;

		private Socket tcpSocket;
		private Socket newTCPSocket;

		private UDTSocket udtSocket;
		private UDTSocket newUDTSocket;

		private byte[] buffer;

		//---- Rates
		private double _currentRate = -1;
		long CurrentTotalBytes;

		private double _rate = -1;
		long TotalBytes;

		Stopwatch _globalStopwatch = new Stopwatch();
		Stopwatch _currentStopwatch = new Stopwatch();

		int _currentRateInterval = 1000;

		//---- Congestion
		private double _averageCwnd;

		//----
		bool _isReliable = true;

		//---- Graph
		private Test.Helper.Windows.LineHandle _perfLineAverage;
		private Test.Helper.Windows.LineHandle _perfLineCurrent;

		//---- Congestion Graph
		private Test.Helper.Windows.LineHandle _perfLineCongestionCurrent;
		private Test.Helper.Windows.LineHandle _perfLineCongestionAverage;

		#endregion

		#region Constructor

		public TestingSocketForm()
		{
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
			InitializeComponent();
			txtIPAddress.Text = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();

			_perfLineAverage = performanceGraph.AddLine("AVERAGE", Color.White);
			_perfLineCurrent = performanceGraph.AddLine("CURRENT", Color.Red);

			_perfLineCongestionAverage = congestionGraph.AddLine("AVERAGE", Color.White);
			_perfLineCongestionCurrent = congestionGraph.AddLine("CURRENT", Color.Red);
		}

		void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			stopUDP = true;
		}

		#endregion

		#region Events...

		private void btnStart_Click(object sender, EventArgs e)
		{
			TotalBytes = 0;
			_globalStopwatch.Stop();
			_globalStopwatch.Reset();

			//---- Send or receive
			IsReceiveMode = (sender == btnStartReceive);

			if (IsReceiveMode)
			{
				btnStart.Enabled = false;
				btnStop.Enabled = false;
				btnStartReceive.Enabled = false;
				this.Text = "Receive packets...";
			}
			else
			{
				btnStart.Enabled = false;
				btnStop.Enabled = true;
				btnStartReceive.Enabled = false;
				this.Text = "Send packets...";
			}

			//---- Create the socket
			if (rbRUDP.Checked)
				rudpSocket = new RUDPSocket();
			else if (rbUDT.Checked)
				udtSocket = new UDTSocket(AddressFamily.InterNetwork);
			else if (rbTCP.Checked)
				tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			else if (rbUDP.Checked)
				tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			//---- Start the benchmark
			if (IsReceiveMode)
				thread = new Thread(new ThreadStart(StartBenchmarkReceive));
			else
				thread = new Thread(new ThreadStart(StartBenchmark));

			thread.Start();

			//---- Data refresh
			if (timer != null)
				timer.Stop();

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 1000;
			timer.Tick += new EventHandler(timer_Tick_Data);
			timer.Start();

			//---- Graph refresh
			if (timerGraph != null)
				timerGraph.Stop();

			timerGraph = new System.Windows.Forms.Timer();
			timerGraph.Interval = 50;
			timerGraph.Tick += new EventHandler(timer_Tick_Graph);
			timerGraph.Start();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			OnTransportDisconnected();

			try
			{
				thread.Abort();
			}
			catch { }

			this.Text = "Waiting action";
			btnStart.Enabled = true;
			btnStop.Enabled = false;
			btnStartReceive.Enabled = true;

			timer.Stop();
			timerGraph.Stop();

			TotalBytes = 0;
			_rate = 0;
			_currentRate = 0;

			_currentStopwatch.Stop();
			_globalStopwatch.Stop();
		}

		#endregion

		#region Timers

		void timer_Tick_Data(object sender, EventArgs e)
		{
			txtRate.Text = ((int)_rate).ToString() + " Kbytes/seconds";
			txtCurrentRate.Text = ((int)_currentRate).ToString() + " KBytes/seconds";

			txtTotalBytes.Text = "" + (TotalBytes / 1024);
			if (!_globalStopwatch.IsRunning)
				txtSeconds.Text = "0";
			else
				txtSeconds.Text = "" + _globalStopwatch.ElapsedMilliseconds / 1000;

			if (rudpSocket != null)
			{
				txtPMTU.Text = rudpSocket.NetworkInformation.PathMTU.ToString();
				txtSCWND.Text = "" + (rudpSocket.NetworkInformation.CongestionWindow);
				txtRTT.Text = ((double)rudpSocket.NetworkInformation.RTT / 1000).ToString();
				txtRTO.Text = ((double)rudpSocket.NetworkInformation.RTO / 1000).ToString();
				txtBDP.Text = (1000.0 * rudpSocket.NetworkInformation.BandWidth / (1024 * 1024)).ToString();
			}
		}

		void timer_Tick_Graph(object sender, EventArgs e)
		{
			if (!chkRefreshGraphs.Checked)
				return;

			if (btnResize.Text == ">")
				return;

			//---- Performance graph
			if (_rate > -1)
				performanceGraph.Push((int)_rate, "AVERAGE");
			if (_currentRate > -1)
				performanceGraph.Push((int)_currentRate, "CURRENT");
			performanceGraph.UpdateGraph();

			//---- Congestion graph
			if (rudpSocket != null)
			{
				congestionGraph.Push((int)_averageCwnd, "AVERAGE");
				congestionGraph.Push((int)rudpSocket.NetworkInformation.CongestionWindow, "CURRENT");
				congestionGraph.UpdateGraph();
			}
		}

		#endregion

		#region SocketConnect

		private void SocketConnect(IPEndPoint endPoint)
		{
			try
			{
				if (rudpSocket != null)
					rudpSocket.Connect(endPoint);
				if (tcpSocket != null)
					tcpSocket.Connect(endPoint);
				if (udtSocket != null)
					udtSocket.Connect(endPoint);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.Message);
				return;
			}
		}

		#endregion

		#region SocketSend

		private void SocketSend(byte[] buffer)
		{
			//---- RUDP
			if (rudpSocket != null)
			{
				RUDPSocketError error = RUDPSocketError.Success;
				rudpSocket.BeginSend(buffer, 0, buffer.Length, out error, new AsyncCallback(EndOfSend), null, _isReliable);

				if (error != RUDPSocketError.Success)
				{
					OnTransportDisconnected();
					return;
				}
			}

			//---- TCP
			else if (tcpSocket != null)
			{
				while (true)
				{
					SocketError error = SocketError.Success;
					tcpSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, out error, new AsyncCallback(EndOfSend), null);

					if (error == SocketError.Success)
						return;

					if (error != SocketError.NoBufferSpaceAvailable)
					{
						OnTransportDisconnected();
						return;
					}

					Thread.Sleep(1);
				}
			}

			//---- UDT
			else if (udtSocket != null)
			{
				while (true)
				{
					SocketError error = SocketError.Success;
					udtSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, out error, new AsyncCallback(EndOfSend), null);

					if (error == SocketError.Success)
						return;

					if (error != SocketError.NoBufferSpaceAvailable)
					{
						OnTransportDisconnected();
						return;
					}

					Thread.Sleep(1);
				}
			}
		}

		#endregion

		#region StartBenchmark

		private void StartBenchmark()
		{
			_isReliable = chkReliable.Checked;

			buffer = new byte[(int)nupMessageSize.Value];

			//---- Prepare
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), Int32.Parse(txtIPPort.Text));

			try
			{
				SocketConnect(endPoint);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.Message);
				return;
			}

			//---- Start timers
			_currentStopwatch.Start();
			_globalStopwatch.Start();

			//---- Send
			SocketSend(buffer);
		}

		#endregion

		#region StartBenchmarkReceive

		private void StartBenchmarkReceive()
		{
			buffer = new byte[(int)nupMessageSize.Value];

			//---- 1 Accept
			if (rbRUDP.Checked)
			{
				rudpSocket = new RUDPSocket();

				rudpSocket.Bind(new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), Int32.Parse(txtIPPort.Text)));

				rudpSocket.BeginAccept(new AsyncCallback(OnAccept), null);
			}
			else if (rbUDT.Checked)
			{
				udtSocket = new UDTSocket(AddressFamily.InterNetwork);

				udtSocket.Bind(new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), Int32.Parse(txtIPPort.Text)));
				udtSocket.Listen(100);

				udtSocket.BeginAccept(new AsyncCallback(OnAccept), null);
			}
			else if (rbTCP.Checked)
			{
				tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				tcpSocket.Bind(new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), Int32.Parse(txtIPPort.Text)));
				tcpSocket.Listen(100);

				tcpSocket.BeginAccept(new AsyncCallback(OnAccept), null);
			}
			else if (rbUDP.Checked)
			{
				EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
				tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				tcpSocket.Bind(new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), Int32.Parse(txtIPPort.Text)));
				newTCPSocket = tcpSocket;
				tcpSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref sender, new AsyncCallback(OnReceive), null);

				_globalStopwatch.Stop();
				_currentStopwatch.Stop();
				_globalStopwatch.Start();
				_currentStopwatch.Start();

				while (Thread.CurrentThread.IsAlive && !stopUDP)
					Thread.Sleep(500);
			}
		}

		#endregion

		#region OnAccept

		internal void OnAccept(IAsyncResult result)
		{
			//---- Get the socket
			if (rudpSocket != null)
			{
				newRUDPSocket = rudpSocket.EndAccept(result);
				rudpSocket.BeginAccept(new AsyncCallback(OnAccept), null);
				newRUDPSocket.BeginReceive(new AsyncCallback(OnReceive), null);
			}
			else if (tcpSocket != null)
			{
				newTCPSocket = tcpSocket.EndAccept(result);
				tcpSocket.BeginAccept(new AsyncCallback(OnAccept), null);
				newTCPSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
			}
			else if (udtSocket != null)
			{
				newUDTSocket = udtSocket.EndAccept(result);
				udtSocket.BeginAccept(new AsyncCallback(OnAccept), null);
				newUDTSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
			}

			_globalStopwatch.Stop();
			_currentStopwatch.Stop();
			_globalStopwatch.Start();
			_currentStopwatch.Start();
		}

		#endregion

		#region OnReceive

		internal void OnReceive(IAsyncResult result)
		{
			//---- Check connection
			if ((newRUDPSocket != null && !newRUDPSocket.Connected) ||
				(newTCPSocket != null && newTCPSocket.ProtocolType == ProtocolType.Tcp && !newTCPSocket.Connected) ||
				(newUDTSocket != null && !newUDTSocket.Connected))
			{
				OnTransportDisconnected();
				return;
			}

			//---- Process the received bytes
			int length = 0;
			try
			{
				if (newRUDPSocket != null)
				{
					byte[] bytes = newRUDPSocket.EndReceive(result);

					//---- End of connection
					if (bytes == null)
					{
						// Connection closed by peer
						OnTransportDisconnected();
						return;
					}
					else length = bytes.Length;
				}
				else if (newTCPSocket != null)
				{
					if (newTCPSocket.ProtocolType == ProtocolType.Tcp)
						length = newTCPSocket.EndReceive(result);
					else
					{
						EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
						length = newTCPSocket.EndReceiveFrom(result, ref sender);
					}
				}
				else
				{
					length = newUDTSocket.EndReceive(result);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);

				OnTransportDisconnected();
				return;
			}

			//---- Current rate
			UpdateRates(length);

			//---- Continue receive packets
			try
			{
				if (newRUDPSocket != null)
					newRUDPSocket.BeginReceive(new AsyncCallback(OnReceive), Environment.TickCount);
				else if (newTCPSocket != null)
				{
					EndPoint sender2 = new IPEndPoint(IPAddress.Any, 0);
					if (newTCPSocket.ProtocolType == ProtocolType.Tcp)
						newTCPSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
					else
						newTCPSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref sender2, new AsyncCallback(OnReceive), null);
				}
				else
					newUDTSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
			}
			catch (Exception)
			{
				OnTransportDisconnected();
				return;
			}
		}

		#endregion

		#region EndOfSend

		private void EndOfSend(IAsyncResult result)
		{
			//---- Get bytes
			int bytes = -1;
			try
			{
				if (rudpSocket != null)
					bytes = rudpSocket.EndSend(result);
				else if (tcpSocket != null)
					bytes = tcpSocket.EndSend(result);
				else if (udtSocket != null)
					bytes = udtSocket.EndSend(result);
			}
			catch (SocketException socketException)
			{
				if (socketException.ErrorCode == 10054)
				{
					OnTransportDisconnected();
					return;
				}

				System.Console.WriteLine("Error " + socketException.Message);
			}
			catch (ObjectDisposedException ode)
			{
				System.Console.WriteLine("Error " + ode.Message);
			}

			//---- Update the rates
			UpdateRates(bytes);

			//---- Send anoter
			SocketSend(buffer);
		}

		#endregion

		#region UpdateRates

		private void UpdateRates(int bytes)
		{
			//---- Current rate
			double seconds;
			CurrentTotalBytes += bytes;
			if (_currentStopwatch.ElapsedMilliseconds > _currentRateInterval)
			{
				seconds = ((double)_currentStopwatch.ElapsedMilliseconds) / 1000;
				_currentRate = ((double)CurrentTotalBytes) / (1024 * seconds);
				_currentStopwatch.Reset();
				_currentStopwatch.Start();
				CurrentTotalBytes = 0;
			}

			//---- Global rate
			TotalBytes += bytes;
			seconds = ((double)_globalStopwatch.ElapsedMilliseconds) / 1000;
			_rate = ((double)TotalBytes) / (1024 * seconds);

			//---- Cwnd
			if (rudpSocket != null)
				_averageCwnd = _averageCwnd * 0.8 + 0.2 * rudpSocket.NetworkInformation.CongestionWindow;
		}

		#endregion

		#region OnTransportDisconnected

		private void OnTransportDisconnected()
		{
			try
			{
				if (rudpSocket != null)
					rudpSocket.Close();
				if (tcpSocket != null)
					tcpSocket.Close();
				if (udtSocket != null)
					udtSocket.Close();
			}
			catch { }
		}

		#endregion

		#region Closing

		private void TCPClientForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			OnTransportDisconnected();
			stopUDP = true;

			try
			{
				thread.Abort();
			}
			catch { }
		}

		private void TestingSocketForm_Load(object sender, EventArgs e)
		{
			btnResize_Click(null, null);
		}

		#endregion

		#region trackBarZoom_Scroll

		private void trackBarZoom_Scroll(object sender, EventArgs e)
		{
			if (trackBarZoom.Value < 1)
				return;
			performanceGraph.MaxPeekMagnitude = trackBarZoom.Value;
			performanceGraph.UpdateGraph();
			performanceGraph.MaxLabel = trackBarZoom.Value + "(Kb/s)";
			labelZoom.Text = "Zoom : " + performanceGraph.MaxPeekMagnitude + " KB/s";
		}

		#endregion

		#region trackBarZoomCongestion_Scroll

		private void trackBarZoomCongestion_Scroll(object sender, EventArgs e)
		{
			if (trackBarZoomCongestion.Value < 1)
				return;
			congestionGraph.MaxPeekMagnitude = trackBarZoomCongestion.Value;
			congestionGraph.UpdateGraph();
			congestionGraph.MaxLabel = trackBarZoomCongestion.Value + "(Kb)";
			labelZoomCongestion.Text = "Zoom : " + performanceGraph.MaxPeekMagnitude + " KB";
		}

		#endregion

		#region btnResize_Click

		private void btnResize_Click(object sender, EventArgs e)
		{
			if (btnResize.Text == "<")
			{
				//---- Set to minimum size
				btnResize.Text = ">";
				this.Width = 285;
			}
			else
			{
				//---- Set to normal size
				btnResize.Text = "<";
				this.Width = 725;
			}
		}

		#endregion

	}
}