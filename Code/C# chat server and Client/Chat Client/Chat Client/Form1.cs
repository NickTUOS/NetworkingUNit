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
using System.Threading;
using ChatSystemCommon;

namespace Chat_Server
{

    

    public partial class Form1 : Form
    {
        private const int SendToServerPort = 1234;
        private int ReviceFromServerPort = 1235;

        BackgroundWorker ListenThread = new BackgroundWorker();
        BackgroundWorker SendThread = new BackgroundWorker();

        bool isLogedin = false;

        ChatUser thisUser;
        ChatMessageProcessor MessageProcessor = new ChatMessageProcessor();

        public Form1()
        {
            InitializeComponent();

            //MessageProcessor.onLoginRequest += new ChatMessageProcessor.OnLoginRequest(LoginRequest);
            MessageProcessor.onLoginSucessful += new ChatMessageProcessor.OnLoginSucessful(LoginSucesfull);
            MessageProcessor.onLoginFailed += new ChatMessageProcessor.OnLoginFailed(Loginfailed);
            MessageProcessor.onMessageAll += new ChatMessageProcessor.OnMessageAll(MessageAllRecived);
            MessageProcessor.onMessageOne += new ChatMessageProcessor.OnMessageOne(MessageOneRecived);
            MessageProcessor.onisAlive += new ChatMessageProcessor.OnisAlive(isAlive);


            SendThread.DoWork += new DoWorkEventHandler(SenderDoWork);
            SendThread.RunWorkerAsync();

            ListenThread.DoWork += new DoWorkEventHandler(ListenerDoWork);
            ListenThread.ProgressChanged += new ProgressChangedEventHandler(Listener_ProgressChanged);
            ListenThread.WorkerReportsProgress = true;
            ListenThread.RunWorkerAsync();

            

            thisUser = new ChatUser("Minty"+DateTime.Now.Ticks.ToString());
            ChatMessage LogonMessage = ChatMessage.MakeLoginRequestMassage(thisUser);
            QueueMessageToSend(LogonMessage);

        }


        void ProcessMessage(ChatMessage message)
        {
            MessageProcessor.ProcessChatMessage(message);
        }

        void LoginRequest(ChatMessage message)
        {
            
        }
        void LoginSucesfull(ChatMessage message)
        {
            isLogedin = true;
            SetText("Login Sucesfull " + message.MessageBody, Color.Green);
        }
        void Loginfailed(ChatMessage message)
        {
            SetText("Login Failed " + message.MessageBody, Color.Red);
        }
        void MessageAllRecived(ChatMessage message)
        {
            SetText(message.user.NickName +": "+ message.MessageBody, Color.Black);
        }
        void MessageOneRecived(ChatMessage message)
        {
            SetText("Whisper from: "+message.user.NickName + ": " + message.MessageBody, Color.Black);
        }
        void isAlive(ChatMessage message)
        {
            message.TypeOfMessage = ChatMessage.MessageType.StillAlive;
            QueueMessageToSend(message);
        }




        /// 
        /// worker send
        ///
        private Queue<ChatMessage> MessagesToSend = new Queue<ChatMessage>();
        private void SenderDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            sendMessage(worker, e);

        }

        

        private void sendMessage(BackgroundWorker worker, DoWorkEventArgs e)
        {
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, SendToServerPort);
           

            while (true)
            {
                while (MessagesToSend.Count > 0)
                {
                    
                    try
                    {
                        Socket ClientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        
                        //connect to end point
                        ClientSocket.Connect(remoteEP);
                        //ReviceFromServerPort= (ClientSocket.LocalEndPoint as IPEndPoint).Port;

                        byte[] byteData = Encoding.ASCII.GetBytes(MessagesToSend.Dequeue().SerializeToXML());
                        ClientSocket.Send(byteData);

                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();

                    }
                    catch (Exception ex)
                    {
                        SetTextAppendNewLine(ex.ToString(), Color.Red);
                    }
                }
            }
        }
        /// 
        /// end worker sendeing
        ///


        /// 
        /// worker
        ///
        private void ListenerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Listen(worker, e);

        }

        int GetFreePort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = (l.LocalEndpoint as IPEndPoint).Port;
            l.Stop();
            return port;
        }

        void Listen(BackgroundWorker worker, DoWorkEventArgs e)
        {
            IPAddress iPAddress = IPAddress.Loopback;// iPHostEntry.AddressList[0];
            ReviceFromServerPort = GetFreePort();
            thisUser.port = ReviceFromServerPort;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, ReviceFromServerPort);
            
            Socket ServerSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(iPEndPoint);
            ServerSocket.Listen(10);

            string data;
            byte[] bytes = new Byte[1024];

            while (true)
            {
                try
                {
                    worker.ReportProgress(0, "listenting for connections");
                    Socket ClientSocket = ServerSocket.Accept();
                    data = null;
                    worker.ReportProgress(0, "Message incomming");


                    int bytseRec = ClientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytseRec);

                    //SetTextAppendNewLine(data);
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();


                    ChatMessage message = ChatMessage.DeserializeFromXML(data);
                    ProcessMessage(message);
                }
                catch(Exception ex)
                {
                    SetTextAppendNewLine(ex.ToString(), Color.Red);
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



        ~Form1()
        {
            
        }

        void QueueMessageToSend(string message)
        {
            ChatMessage cm = new ChatMessage();
            cm.MessageBody = message;
            cm.TypeOfMessage = ChatMessage.MessageType.MessageAll;
            QueueMessageToSend(cm);
        }
        void QueueMessageToSend(ChatMessage message)
        { 
            MessagesToSend.Enqueue(message);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            QueueMessageToSend(txtMessage.Text);
            txtMessage.Clear();
        }

        void Send(string Message)
        {
            if (isLogedin)
            {
                QueueMessageToSend(txtMessage.Text);
                txtMessage.Clear();
            }
            else
            {
                SetTextAppendNewLine("Your are not yet loged on to the server: please log on", Color.Red);
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
            if(this.txtRecivedMessages.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, textColor });
            }
            else
            {
                int startLength = txtRecivedMessages.TextLength;
                this.txtRecivedMessages.AppendText(text);
                int endLength = txtRecivedMessages.TextLength;
                this.txtRecivedMessages.Select(startLength, endLength);
                this.txtRecivedMessages.SelectionColor = textColor ?? Color.Black;
                this.txtRecivedMessages.SelectionLength = 0;
                this.txtRecivedMessages.SelectionColor = Color.Black;
                this.txtRecivedMessages.ScrollToCaret();
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Send(txtMessage.Text);
            }
        }
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
