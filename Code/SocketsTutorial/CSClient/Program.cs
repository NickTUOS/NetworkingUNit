using System;
using System.Net;
using System.Net.Sockets;
using System.Text;



namespace CSClient
{
    class Client
    {
        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                //IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddress = IPAddress.Loopback;
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1234); // the sever we want to connect to

                Socket senderSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    senderSocket.Connect(iPEndPoint);
                    Console.WriteLine("Socket Connected to {0}", senderSocket.RemoteEndPoint.ToString());

                    string sMessage = Console.ReadLine();

                    byte[] msg = Encoding.ASCII.GetBytes(sMessage);

                    int bytesSent = senderSocket.Send(msg);

                    int bytesRec = senderSocket.Receive(bytes);

                    Console.WriteLine("Enchoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    senderSocket.Shutdown(SocketShutdown.Both);
                    senderSocket.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullExeption : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketExeption : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            } //end of first Try block
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }



        static void Main(string[] args)
        {
            while (true)
            {
                StartClient();
            }
            Console.WriteLine("Client shuting down");
            Console.ReadLine();
        }
    }// end of class
}//end of name space
