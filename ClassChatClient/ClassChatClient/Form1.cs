using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace ClassChatClient
{

    public partial class Form1 : Form
    {
        //our listener thread.
        // we will use this to continuasly listen for incomming messages.
        BackgroundWorker ListenThread = new BackgroundWorker();


        //ctor
        public Form1()
        {
            //.NET init function. sets up my GUI for me.
            InitializeComponent();

            //link a function to the threads do work event.
            ListenThread.DoWork += new DoWorkEventHandler(ListenerDoWork);
            //start the thread.
            ListenThread.RunWorkerAsync();
        }


        //our function for listening for incomming data.
        //function name is unimportant but the return and peramiters ARE important!
        void ListenerDoWork(object sender, DoWorkEventArgs e)
        {
            //youve seen the this code befor.
            //only thing you need to rememeber is the while true loop.
            IPAddress iPAddress = IPAddress.Loopback;// iPHostEntry.AddressList[0];


            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1235);

            Socket ServerSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(iPEndPoint);
            ServerSocket.Listen(10);

            string data;
            byte[] bytes = new Byte[1024];

            while (true)
            {
                try
                {
                    Socket ClientSocket = ServerSocket.Accept();
                    data = null;

                    int bytseRec = ClientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytseRec);

                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();

                    //call teh set tet function to add our message to the rtbRecive tet box
                    SetText(data.ToString());
                }
                catch (Exception ex)
                {
                    SetText(ex.ToString());
                }
            }
        }

        //function constructed by .NET for our button.
        //again return and peramiters are important.
        private void btnSend_Click(object sender, EventArgs e)
        {
            //nothing new here
            try
            {
                IPAddress IP = IPAddress.Loopback;
                IPEndPoint remoteEP = new IPEndPoint(IP, 1234);

                Socket ClientSocket = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                ClientSocket.Connect(remoteEP);

                byte[] data = Encoding.ASCII.GetBytes(rtbMessage.Text);
                ClientSocket.Send(data);
            }
            catch (Exception ex)
            {
                //becouse this is executing in the main thread, we dont need to call settext, but we could.
                rtbRecived.Text += ex.ToString();
            }
        }

        //basicaly, an event we can call from any thread.
        delegate void SetRTBRecived(string Text);
        //the function we will call on the delagete
        //return and peramiter are important.
        void SetText(string text)
        {
            //test to see if this thread owns the controle.
            if(this.rtbRecived.InvokeRequired) //if no
            {
                //make a new instance of the delageate, link it to this function
                //the next thread in the chain (main) should pick it up and set text
                SetRTBRecived setTextDelegate = new SetRTBRecived(SetText);
                //call the event
                this.Invoke(setTextDelegate, new object [] { text });
            }
            else // if this thread owns the controle (main)
            {
                rtbRecived.Text += text;
            }
        }
    }
}
