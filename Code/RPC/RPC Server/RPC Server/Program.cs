using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RPC_Server
{
    class Program
    {
        static void Main(string[] args)
        {

            Dictionary<string, Func<double, double, double>> methods = new Dictionary<string, Func<double, double, double>>();
            methods.Add("add", Add);
            methods.Add("sub", Sub);
            methods.Add("mult", Mult);
            methods.Add("div", Div);
            methods.Add("pow", Pow);


            IPHostEntry ipHostEntery = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            foreach (IPAddress ip in ipHostEntery.AddressList)
            {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                    break;
                }
            }
            if(ipAddress== null)
            {
                Console.WriteLine("ERROR: NO IP4 ADDRESS!!");
                Console.ReadLine();
                return;
            }
            IPEndPoint ServerEndPoint = new IPEndPoint(ipAddress, 1234);
            Socket ServerSocketBinding = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                ServerSocketBinding.Bind(ServerEndPoint);
                ServerSocketBinding.Listen(10);
                while(true)
                {
                    Console.WriteLine("Server is Waiting for connection");
                    Socket ClientSocket = ServerSocketBinding.Accept();

                    byte[] bytes = new byte[1024]; ;
                    int bytseRec = ClientSocket.Receive(bytes);

                    RPC_Marsheller.RPCObject RPCData;

                    using (var memStream = new MemoryStream())
                    {
                        var binForm = new BinaryFormatter();
                        memStream.Write(bytes, 0, bytes.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                        RPCData = (RPC_Marsheller.RPCObject)binForm.Deserialize(memStream);
                    }

                    double[] perams;
                    using (var memStream = new MemoryStream())
                    {
                        var binForm = new BinaryFormatter();
                        memStream.Write(RPCData.data, 0, RPCData.data.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                        perams = (double[])binForm.Deserialize(memStream);
                    }

                    if(perams.Length != 2)
                    {
                        Console.WriteLine("ERROR: NOT ENOUGH PERAMITERS!");
                    }
                    else
                    {
                        double result = methods[RPCData.RemoteMethordName.ToLower()](perams[0], perams[1]);
                        byte[] resultBytes = new byte[1024];
                        BinaryFormatter bf = new BinaryFormatter();
                        using (var ms = new MemoryStream())
                        {
                            bf.Serialize(ms, result);
                            resultBytes = ms.ToArray();
                        }

                        ClientSocket.Send(resultBytes);
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                    }
                    
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Server Shutting down");
            Console.ReadLine();

            
            
        }

        static double Add(double a, double B)
        {
            return a + B;
        }
        static double Sub(double a, double B)
        {
            return a -B;
        }
        static double Mult(double a, double B)
        {
            return a * B;
        }
        static double Div(double a, double B)
        {
            return a / B;
        }
        static double Pow(double a, double B)
        {
            return Math.Pow(a,B);
        }
        
    }
}
