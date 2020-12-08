using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using ChatSystemCommon;

namespace ChatClient
{
    public partial class ServerMain : Form
    {
        private const int ReciveFromClientPort = 1234;
        private const int SendToClientPort = 1235;

        BackgroundWorker ListenThread = new BackgroundWorker();

        ChatMessageProcessor MessageProcessor = new ChatMessageProcessor();

        Dictionary<string, ChatUser> ConnetedUsersDict = new Dictionary<string, ChatUser>();

        int ConnectionTimeoutTime = 10;

        public ServerMain()
        {
            InitializeComponent();

            MessageProcessor.onLoginRequest += new ChatMessageProcessor.OnLoginRequest(LoginRequest);
            MessageProcessor.onMessageAll += new ChatMessageProcessor.OnMessageAll(MessageAll);
            MessageProcessor.onStillAlive += new ChatMessageProcessor.OnStillAlive(StillAlive);

            ListenThread.DoWork += new DoWorkEventHandler(ListenerDoWork);
            ListenThread.ProgressChanged += new ProgressChangedEventHandler(Listener_ProgressChanged);
            ListenThread.WorkerReportsProgress = true;
            ListenThread.RunWorkerAsync();
        }

        void ProcessMessage(ChatMessage message)
        {
            try
            {
                MessageProcessor.ProcessChatMessage(message);
            }
            catch (Exception e)
            {
                SetText(e.ToString(), Color.Red);
            }
        }

        void LoginRequest(ChatMessage message)
        {
            ChatMessage m;
            if (ConnetedUsersDict.ContainsKey(message.user.NickName))
            {
                m = ChatMessage.MakeLoginFailedMessage("Another user with that name already exists!");
            }
            else
            {
                ConnetedUsersDict.Add(message.user.NickName, message.user);
                m = ChatMessage.MakeLoginSucessfulMessage("Welcome to the server " + message.user.NickName + "!");
            }
            m.user = message.user;
            SendData(m);
        }
        void MessageAll(ChatMessage message)
        {
            foreach(ChatUser user in ConnetedUsersDict.Values)
            {
                message.user = user;
                SendData(message);
            }
        }
        void StillAlive(ChatMessage message)
        {
            //what am i doing here? 
            //im trying to see if user is in dict but this is async task so i need to mark user as present?
            //time since last alive?
            //if greater than time out.... where to store time out?
            ConnetedUsersDict[message.user.NickName].LastAliveTime = DateTime.Now.Ticks;
        }


        /// 
        /// worker
        ///
        private void ListenerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Listen(worker, e);

        }
        void Listen(BackgroundWorker worker, DoWorkEventArgs e)
        {
            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, ReciveFromClientPort);

            


            Socket ServerSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(iPEndPoint);
            ServerSocket.Listen(10);

            string data = null;
            byte[] bytes = new Byte[1024];

            while (true)
            {
                CheckUsersAlive();

                try
                {
                    worker.ReportProgress(0, "listenting for connections");
                    Socket ClientSocket = ServerSocket.Accept();
                    data = null;
                    worker.ReportProgress(0, "Message incomming");


                    int bytseRec = ClientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytseRec);


                    //SetTextAppendNewLine(data);
                    ChatMessage message = ChatMessage.DeserializeFromXML(data);
                    message.user.ipAddress = (ClientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                    //message.user.port = (ClientSocket.RemoteEndPoint as IPEndPoint).Port;
                    ProcessMessage(message);

                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                }
                catch (Exception ex)
                {
                    SetTextAppendNewLine(ex.ToString(), Color.Red);
                }


                
            }
        }

        void CheckUsersAlive()
        { 
            ChatMessage isAliveMessage = ChatMessage.MakeisAliveMessage();

            Dictionary<string, ChatUser> temp = new Dictionary<string, ChatUser>(ConnetedUsersDict);
            foreach (ChatUser user in temp.Values)
            {
                
                if (user.LastAliveTime < DateTime.Now.AddSeconds(-ConnectionTimeoutTime*2).Ticks)
                    ConnetedUsersDict.Remove(user.NickName);
                if (user.LastAliveTime < DateTime.Now.AddSeconds(-ConnectionTimeoutTime).Ticks)
                {
                    isAliveMessage.user = user;
                    SendData(isAliveMessage);
                }
            }
        }

        private void Listener_ProgressChanged(object sender,
           ProgressChangedEventArgs e)
        {
            string progresMessage = e.UserState as string;
            //SetTextAppendNewLine(e.ProgressPercentage.ToString() + progresMessage);
        }
        /// 
        /// end worker
        /// 

        //TODO make this take user info and send acordingly
        void SendData(ChatMessage data)
        {
            IPAddress ClientIPAddress = IPAddress.Parse(data.user.ipAddress);
            IPEndPoint CliendEndPoint = new IPEndPoint(ClientIPAddress, data.user.port); //SendToClientPort
            try
            {
                Socket ClientSocket = new Socket(ClientIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //connect to end point
                ClientSocket.Connect(CliendEndPoint);

                byte[] byteData = Encoding.ASCII.GetBytes(data.SerializeToXML());

                ClientSocket.Send(byteData);

                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }
            catch (Exception ex)
            {
                SetTextAppendNewLine(ex.ToString(), Color.Red);
            }
        }

        void SetTextAppendNewLine(string text, Color? textColor = null)
        {
            SetText(text + Environment.NewLine, textColor);
        }
        void SetTextPrependNewLine(string text, Color? textColor = null)
        {
            SetText(text + Environment.NewLine, textColor);
        }

        delegate void SetTextCallback(string text, Color? textColor);
        private void SetText(string text, Color? textColor = null)
        {
            if (this.rtbLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, textColor });
            }
            else
            {
                int startLength = rtbLog.TextLength;
                this.rtbLog.AppendText(text);
                int endLength = rtbLog.TextLength;
                this.rtbLog.Select(startLength, endLength);
                this.rtbLog.SelectionColor = textColor ?? Color.Black;
                this.rtbLog.SelectionLength = 0;
                this.rtbLog.SelectionColor = Color.Black;
                this.rtbLog.ScrollToCaret();
            }
        }
    }
}
