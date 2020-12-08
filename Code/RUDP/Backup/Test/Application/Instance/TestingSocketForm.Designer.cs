namespace Test.Application
{
	partial class TestingSocketForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStart = new System.Windows.Forms.Button();
			this.txtIPAddress = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtIPPort = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbUDP = new System.Windows.Forms.RadioButton();
			this.rbUDT = new System.Windows.Forms.RadioButton();
			this.rbTCP = new System.Windows.Forms.RadioButton();
			this.rbRUDP = new System.Windows.Forms.RadioButton();
			this.chkReliable = new System.Windows.Forms.CheckBox();
			this.txtRate = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.txtTotalBytes = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSeconds = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtBDP = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtRTO = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtRTT = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtSCWND = new System.Windows.Forms.TextBox();
			this.txtPMTU = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.labelZoom = new System.Windows.Forms.Label();
			this.trackBarZoom = new System.Windows.Forms.TrackBar();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.labelZoomCongestion = new System.Windows.Forms.Label();
			this.trackBarZoomCongestion = new System.Windows.Forms.TrackBar();
			this.chkRefreshGraphs = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label11 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtCurrentRate = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.nupMessageSize = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.btnStartReceive = new System.Windows.Forms.Button();
			this.btnResize = new System.Windows.Forms.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.performanceGraph = new Test.Helper.Windows.PerformanceGraph();
			this.congestionGraph = new Test.Helper.Windows.PerformanceGraph();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoomCongestion)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nupMessageSize)).BeginInit();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(10, 15);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start...";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// txtIPAddress
			// 
			this.txtIPAddress.Location = new System.Drawing.Point(75, 38);
			this.txtIPAddress.Name = "txtIPAddress";
			this.txtIPAddress.Size = new System.Drawing.Size(100, 20);
			this.txtIPAddress.TabIndex = 3;
			this.txtIPAddress.Text = "172.22.41.81";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Server IP:";
			// 
			// txtIPPort
			// 
			this.txtIPPort.Location = new System.Drawing.Point(181, 38);
			this.txtIPPort.Name = "txtIPPort";
			this.txtIPPort.Size = new System.Drawing.Size(29, 20);
			this.txtIPPort.TabIndex = 5;
			this.txtIPPort.Text = "100";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbUDP);
			this.groupBox1.Controls.Add(this.rbUDT);
			this.groupBox1.Controls.Add(this.rbTCP);
			this.groupBox1.Controls.Add(this.rbRUDP);
			this.groupBox1.Controls.Add(this.txtIPPort);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtIPAddress);
			this.groupBox1.Location = new System.Drawing.Point(11, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 68);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// rbUDP
			// 
			this.rbUDP.AutoSize = true;
			this.rbUDP.Location = new System.Drawing.Point(176, 19);
			this.rbUDP.Name = "rbUDP";
			this.rbUDP.Size = new System.Drawing.Size(48, 17);
			this.rbUDP.TabIndex = 39;
			this.rbUDP.Text = "UDP";
			this.rbUDP.UseVisualStyleBackColor = true;
			// 
			// rbUDT
			// 
			this.rbUDT.AutoSize = true;
			this.rbUDT.Location = new System.Drawing.Point(72, 19);
			this.rbUDT.Name = "rbUDT";
			this.rbUDT.Size = new System.Drawing.Size(48, 17);
			this.rbUDT.TabIndex = 38;
			this.rbUDT.Text = "UDT";
			this.rbUDT.UseVisualStyleBackColor = true;
			// 
			// rbTCP
			// 
			this.rbTCP.AutoSize = true;
			this.rbTCP.Location = new System.Drawing.Point(124, 19);
			this.rbTCP.Name = "rbTCP";
			this.rbTCP.Size = new System.Drawing.Size(46, 17);
			this.rbTCP.TabIndex = 37;
			this.rbTCP.Text = "TCP";
			this.rbTCP.UseVisualStyleBackColor = true;
			// 
			// rbRUDP
			// 
			this.rbRUDP.AutoSize = true;
			this.rbRUDP.Checked = true;
			this.rbRUDP.Location = new System.Drawing.Point(15, 19);
			this.rbRUDP.Name = "rbRUDP";
			this.rbRUDP.Size = new System.Drawing.Size(56, 17);
			this.rbRUDP.TabIndex = 36;
			this.rbRUDP.TabStop = true;
			this.rbRUDP.Text = "RUDP";
			this.rbRUDP.UseVisualStyleBackColor = true;
			// 
			// chkReliable
			// 
			this.chkReliable.AutoSize = true;
			this.chkReliable.Checked = true;
			this.chkReliable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkReliable.Location = new System.Drawing.Point(10, 44);
			this.chkReliable.Name = "chkReliable";
			this.chkReliable.Size = new System.Drawing.Size(75, 17);
			this.chkReliable.TabIndex = 39;
			this.chkReliable.Text = "Is Reliable";
			this.chkReliable.UseVisualStyleBackColor = true;
			// 
			// txtRate
			// 
			this.txtRate.Location = new System.Drawing.Point(93, 282);
			this.txtRate.Name = "txtRate";
			this.txtRate.Size = new System.Drawing.Size(140, 20);
			this.txtRate.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 285);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Average rate:";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(134, 17);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 14;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// txtTotalBytes
			// 
			this.txtTotalBytes.Location = new System.Drawing.Point(93, 334);
			this.txtTotalBytes.Name = "txtTotalBytes";
			this.txtTotalBytes.Size = new System.Drawing.Size(140, 20);
			this.txtTotalBytes.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 337);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Total Kbytes:";
			// 
			// txtSeconds
			// 
			this.txtSeconds.Location = new System.Drawing.Point(93, 360);
			this.txtSeconds.Name = "txtSeconds";
			this.txtSeconds.Size = new System.Drawing.Size(140, 20);
			this.txtSeconds.TabIndex = 18;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 363);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Seconds :";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtBDP);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.txtRTO);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtRTT);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.txtSCWND);
			this.groupBox2.Controls.Add(this.txtPMTU);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Location = new System.Drawing.Point(11, 392);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(240, 143);
			this.groupBox2.TabIndex = 19;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Network information";
			// 
			// txtBDP
			// 
			this.txtBDP.Location = new System.Drawing.Point(88, 117);
			this.txtBDP.Name = "txtBDP";
			this.txtBDP.ReadOnly = true;
			this.txtBDP.Size = new System.Drawing.Size(100, 20);
			this.txtBDP.TabIndex = 13;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(14, 120);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(78, 13);
			this.label9.TabIndex = 12;
			this.label9.Text = "BDP (Mb/sec):";
			// 
			// txtRTO
			// 
			this.txtRTO.Location = new System.Drawing.Point(88, 91);
			this.txtRTO.Name = "txtRTO";
			this.txtRTO.ReadOnly = true;
			this.txtRTO.Size = new System.Drawing.Size(100, 20);
			this.txtRTO.TabIndex = 11;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(14, 94);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(55, 13);
			this.label8.TabIndex = 10;
			this.label8.Text = "RTO (ms):";
			// 
			// txtRTT
			// 
			this.txtRTT.Location = new System.Drawing.Point(88, 65);
			this.txtRTT.Name = "txtRTT";
			this.txtRTT.ReadOnly = true;
			this.txtRTT.Size = new System.Drawing.Size(100, 20);
			this.txtRTT.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(14, 68);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(54, 13);
			this.label7.TabIndex = 8;
			this.label7.Text = "RTT (ms):";
			// 
			// txtSCWND
			// 
			this.txtSCWND.Location = new System.Drawing.Point(88, 39);
			this.txtSCWND.Name = "txtSCWND";
			this.txtSCWND.ReadOnly = true;
			this.txtSCWND.Size = new System.Drawing.Size(100, 20);
			this.txtSCWND.TabIndex = 7;
			// 
			// txtPMTU
			// 
			this.txtPMTU.Location = new System.Drawing.Point(88, 16);
			this.txtPMTU.Name = "txtPMTU";
			this.txtPMTU.ReadOnly = true;
			this.txtPMTU.Size = new System.Drawing.Size(100, 20);
			this.txtPMTU.TabIndex = 6;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(14, 42);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(54, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "SCWND :";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(62, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Path MTU :";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(280, 63);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(436, 473);
			this.tabControl1.TabIndex = 35;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.performanceGraph);
			this.tabPage1.Controls.Add(this.labelZoom);
			this.tabPage1.Controls.Add(this.trackBarZoom);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(428, 447);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Speed Kb/s";
			// 
			// labelZoom
			// 
			this.labelZoom.AutoSize = true;
			this.labelZoom.Location = new System.Drawing.Point(13, 429);
			this.labelZoom.Name = "labelZoom";
			this.labelZoom.Size = new System.Drawing.Size(99, 13);
			this.labelZoom.TabIndex = 31;
			this.labelZoom.Text = "Zoom : 65000 Kb/s";
			// 
			// trackBarZoom
			// 
			this.trackBarZoom.Location = new System.Drawing.Point(3, 397);
			this.trackBarZoom.Maximum = 200000;
			this.trackBarZoom.Name = "trackBarZoom";
			this.trackBarZoom.Size = new System.Drawing.Size(426, 45);
			this.trackBarZoom.TabIndex = 30;
			this.trackBarZoom.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBarZoom.Value = 65000;
			this.trackBarZoom.Scroll += new System.EventHandler(this.trackBarZoom_Scroll);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.congestionGraph);
			this.tabPage2.Controls.Add(this.labelZoomCongestion);
			this.tabPage2.Controls.Add(this.trackBarZoomCongestion);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(428, 447);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Congestion window";
			// 
			// labelZoomCongestion
			// 
			this.labelZoomCongestion.AutoSize = true;
			this.labelZoomCongestion.Location = new System.Drawing.Point(16, 429);
			this.labelZoomCongestion.Name = "labelZoomCongestion";
			this.labelZoomCongestion.Size = new System.Drawing.Size(89, 13);
			this.labelZoomCongestion.TabIndex = 31;
			this.labelZoomCongestion.Text = "Zoom : 65000 Kb";
			// 
			// trackBarZoomCongestion
			// 
			this.trackBarZoomCongestion.Location = new System.Drawing.Point(6, 397);
			this.trackBarZoomCongestion.Maximum = 120000;
			this.trackBarZoomCongestion.Name = "trackBarZoomCongestion";
			this.trackBarZoomCongestion.Size = new System.Drawing.Size(426, 45);
			this.trackBarZoomCongestion.TabIndex = 30;
			this.trackBarZoomCongestion.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackBarZoomCongestion.Value = 65000;
			this.trackBarZoomCongestion.Scroll += new System.EventHandler(this.trackBarZoomCongestion_Scroll);
			// 
			// chkRefreshGraphs
			// 
			this.chkRefreshGraphs.AutoSize = true;
			this.chkRefreshGraphs.Checked = true;
			this.chkRefreshGraphs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRefreshGraphs.Location = new System.Drawing.Point(280, 12);
			this.chkRefreshGraphs.Name = "chkRefreshGraphs";
			this.chkRefreshGraphs.Size = new System.Drawing.Size(98, 17);
			this.chkRefreshGraphs.TabIndex = 36;
			this.chkRefreshGraphs.Text = "Refresh graphs";
			this.chkRefreshGraphs.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 31);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(111, 13);
			this.label10.TabIndex = 40;
			this.label10.Text = "Current speed( Kb/s) :";
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.White;
			this.panel3.Location = new System.Drawing.Point(120, 24);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(100, 1);
			this.panel3.TabIndex = 39;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(117, 13);
			this.label11.TabIndex = 38;
			this.label11.Text = "Average speed( Kb/s) :";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.panel2.Location = new System.Drawing.Point(120, 39);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(100, 1);
			this.panel2.TabIndex = 37;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.panel2);
			this.groupBox3.Controls.Add(this.panel3);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Location = new System.Drawing.Point(478, 7);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(228, 50);
			this.groupBox3.TabIndex = 41;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = " Legend ";
			// 
			// txtCurrentRate
			// 
			this.txtCurrentRate.Location = new System.Drawing.Point(93, 308);
			this.txtCurrentRate.Name = "txtCurrentRate";
			this.txtCurrentRate.Size = new System.Drawing.Size(140, 20);
			this.txtCurrentRate.TabIndex = 43;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(16, 311);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 13);
			this.label12.TabIndex = 42;
			this.label12.Text = "Current rate:";
			// 
			// groupBox4
			// 
			this.groupBox4.Location = new System.Drawing.Point(11, 266);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(240, 120);
			this.groupBox4.TabIndex = 44;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Speed information";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.btnStart);
			this.groupBox5.Controls.Add(this.btnStop);
			this.groupBox5.Controls.Add(this.chkReliable);
			this.groupBox5.Location = new System.Drawing.Point(12, 86);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(239, 71);
			this.groupBox5.TabIndex = 45;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Send...";
			// 
			// nupMessageSize
			// 
			this.nupMessageSize.Increment = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.nupMessageSize.Location = new System.Drawing.Point(123, 18);
			this.nupMessageSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.nupMessageSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nupMessageSize.Name = "nupMessageSize";
			this.nupMessageSize.Size = new System.Drawing.Size(86, 20);
			this.nupMessageSize.TabIndex = 42;
			this.nupMessageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.nupMessageSize.Value = new decimal(new int[] {
            32768,
            0,
            0,
            0});
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(7, 20);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(97, 13);
			this.label13.TabIndex = 40;
			this.label13.Text = "Buffer size (Bytes) :";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.btnStartReceive);
			this.groupBox6.Location = new System.Drawing.Point(12, 216);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(240, 44);
			this.groupBox6.TabIndex = 46;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Receive...";
			// 
			// btnStartReceive
			// 
			this.btnStartReceive.Location = new System.Drawing.Point(8, 15);
			this.btnStartReceive.Name = "btnStartReceive";
			this.btnStartReceive.Size = new System.Drawing.Size(75, 23);
			this.btnStartReceive.TabIndex = 1;
			this.btnStartReceive.Text = "Start...";
			this.btnStartReceive.UseVisualStyleBackColor = true;
			this.btnStartReceive.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnResize
			// 
			this.btnResize.BackColor = System.Drawing.Color.SlateGray;
			this.btnResize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResize.Location = new System.Drawing.Point(255, 12);
			this.btnResize.Name = "btnResize";
			this.btnResize.Size = new System.Drawing.Size(23, 517);
			this.btnResize.TabIndex = 47;
			this.btnResize.Text = "<";
			this.btnResize.UseVisualStyleBackColor = false;
			this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.nupMessageSize);
			this.groupBox7.Controls.Add(this.label13);
			this.groupBox7.Location = new System.Drawing.Point(12, 166);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(240, 44);
			this.groupBox7.TabIndex = 48;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Memory";
			// 
			// performanceGraph
			// 
			this.performanceGraph.AutoAdjustPeek = false;
			this.performanceGraph.BackColor = System.Drawing.Color.Black;
			this.performanceGraph.GridColor = System.Drawing.Color.Green;
			this.performanceGraph.GridSize = ((ushort)(15));
			this.performanceGraph.HighQuality = false;
			this.performanceGraph.LineInterval = ((ushort)(1));
			this.performanceGraph.Location = new System.Drawing.Point(3, 3);
			this.performanceGraph.MaxLabel = "65000 (Kb/s)";
			this.performanceGraph.MaxPeekMagnitude = 65000;
			this.performanceGraph.MinLabel = "0";
			this.performanceGraph.MinPeekMagnitude = 0;
			this.performanceGraph.Name = "performanceGraph";
			this.performanceGraph.ShowGrid = true;
			this.performanceGraph.ShowLabels = true;
			this.performanceGraph.Size = new System.Drawing.Size(422, 388);
			this.performanceGraph.TabIndex = 32;
			this.performanceGraph.Text = "performanceGraph1";
			this.performanceGraph.TextColor = System.Drawing.Color.Yellow;
			// 
			// congestionGraph
			// 
			this.congestionGraph.AutoAdjustPeek = false;
			this.congestionGraph.BackColor = System.Drawing.Color.Black;
			this.congestionGraph.GridColor = System.Drawing.Color.Green;
			this.congestionGraph.GridSize = ((ushort)(15));
			this.congestionGraph.HighQuality = false;
			this.congestionGraph.LineInterval = ((ushort)(1));
			this.congestionGraph.Location = new System.Drawing.Point(3, 3);
			this.congestionGraph.MaxLabel = "65000 (Kb)";
			this.congestionGraph.MaxPeekMagnitude = 65000;
			this.congestionGraph.MinLabel = "0";
			this.congestionGraph.MinPeekMagnitude = 0;
			this.congestionGraph.Name = "congestionGraph";
			this.congestionGraph.ShowGrid = true;
			this.congestionGraph.ShowLabels = true;
			this.congestionGraph.Size = new System.Drawing.Size(422, 388);
			this.congestionGraph.TabIndex = 32;
			this.congestionGraph.Text = "performanceGraph1";
			this.congestionGraph.TextColor = System.Drawing.Color.Yellow;
			// 
			// TestingSocketForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(719, 541);
			this.Controls.Add(this.btnResize);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.txtCurrentRate);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.chkRefreshGraphs);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.txtSeconds);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtTotalBytes);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtRate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox7);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "TestingSocketForm";
			this.Text = "RUDP Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPClientForm_FormClosing);
			this.Load += new System.EventHandler(this.TestingSocketForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoomCongestion)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nupMessageSize)).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox txtIPAddress;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtIPPort;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtRate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.TextBox txtTotalBytes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSeconds;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbTCP;
		private System.Windows.Forms.RadioButton rbRUDP;
		private System.Windows.Forms.RadioButton rbUDT;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtSCWND;
		private System.Windows.Forms.TextBox txtPMTU;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkReliable;
		private System.Windows.Forms.TextBox txtRTO;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtRTT;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtBDP;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private Test.Helper.Windows.PerformanceGraph performanceGraph;
		private System.Windows.Forms.Label labelZoom;
		private System.Windows.Forms.TrackBar trackBarZoom;
		private System.Windows.Forms.TabPage tabPage2;
		private Test.Helper.Windows.PerformanceGraph congestionGraph;
		private System.Windows.Forms.Label labelZoomCongestion;
		private System.Windows.Forms.TrackBar trackBarZoomCongestion;
		private System.Windows.Forms.CheckBox chkRefreshGraphs;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtCurrentRate;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button btnStartReceive;
		private System.Windows.Forms.Button btnResize;
		private System.Windows.Forms.RadioButton rbUDP;
		private System.Windows.Forms.NumericUpDown nupMessageSize;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.GroupBox groupBox7;
	}
}

