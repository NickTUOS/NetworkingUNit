using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSServer
{
    class Server
    {
        public static string data;

        public static void StartListening()
        {
            byte[] bytes = new Byte[1024];

            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = IPAddress.Loopback;
            //long l =iPHostEntry.AddressList[0].Address;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1234);

            Socket ServerSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                ServerSocket.Bind(iPEndPoint);
                ServerSocket.Listen(10); 

                while(true)
                {
                    Console.WriteLine("Waiting for a connection");
                    Socket ClientSocket = ServerSocket.Accept();

                    data = null;

                    int bytseRec = ClientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytseRec);

                    Console.WriteLine("Text recived: {0}", data);

                    data += " this is form the server!!!";

                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    //ClientSocket.Send(msg);
                    


                    try
                    {
                        ClientSocket.Send(msg);
                        //IPAddress IP = IPAddress.Loopback;
                        //IPEndPoint remoteEP = new IPEndPoint(IP, 1234);

                        //Socket ClientSendSocket = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        //Socket ClientSendSocket = new Socket(ClientSocket.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        //ClientSendSocket.Connect(remoteEP);
                        //ClientSendSocket.Connect(ClientSocket.RemoteEndPoint);
                        //ClientSendSocket.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();



                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }
            Console.WriteLine("/nPress Enter to continue...");
            Console.Read();
        }

        static void Main(string[] args)
        {
            StartListening();
        }
    }//end of class
}//end of name space




////while(true)
////{
//int bytseRec = ClientSocket.Receive(bytes);
//data += Encoding.ASCII.GetString(bytes, 0, bytseRec);
//                        //if (data.IndexOf("<EOF>") > -1)
//                            //break;
//                    //}
