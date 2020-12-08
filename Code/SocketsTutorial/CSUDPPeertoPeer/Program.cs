using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CSUDPPeertoPeer
{
    class Program
    {




        static void Main(string[] args)
        {
            int ServerPort = 1234;
            UdpClient udpclient = new UdpClient();
            udpclient.Client.Bind(new IPEndPoint(IPAddress.Any, ServerPort));

            IPEndPoint from = new IPEndPoint(0, 0);

            Task.Run(() =>
            {
                while (true)
                {
                    var reciveBuffer = udpclient.Receive(ref from);
                    Console.WriteLine("Message from "+
                        from.Address.ToString()+":"+from.Port.ToString()+" "+
                        Encoding.UTF8.GetString(reciveBuffer));
                }
            });


            while (true)
            {
                var data = Encoding.UTF8.GetBytes(Console.ReadLine());
                udpclient.Send(data, data.Length, "255.255.255.255", ServerPort);

                Console.WriteLine("any key to continue:");
                Console.ReadLine();
            }
        }

    }
}
