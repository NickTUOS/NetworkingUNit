using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPC_Calculator
{
    public partial class Form1 : Form
    {
        enum MyCommendTypes
        {
            ADD,
            SUB,
            MULT,
            DIV,
            POW,
            NONE
        }
        MyCommendTypes Command = MyCommendTypes.NONE;
        double RunningTotal = 0;
        public Form1()
        {
            InitializeComponent();
        }

        double Calculate(RPC_Marsheller.RPCObject RPCData)
        {
            double result = 0.0;
            IPHostEntry ipHostEntery = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            foreach (IPAddress ip in ipHostEntery.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                    break;
                }
            }
            if (ipAddress == null)
            {
                Console.WriteLine("ERROR: NO IP4 ADDRESS!!");
                Console.ReadLine();
                return 0.0;
            }
            IPEndPoint ServerEndPoint = new IPEndPoint(ipAddress, 1234);
            Socket ServerSocketBinding = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                ServerSocketBinding.Connect(ServerEndPoint);

                byte[] resultBytes = new byte[1024];
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, RPCData);
                    resultBytes = ms.ToArray();
                }
                ServerSocketBinding.Send(resultBytes);
                int ByteCount = ServerSocketBinding.Receive(resultBytes);

                //convert back in to double result
                
                using (var memStream = new MemoryStream())
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(resultBytes, 0, resultBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    result = (double)binForm.Deserialize(memStream);
                }

                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }


        private void anyNumBtn_Click(object sender, EventArgs e)
        {
            if(Command == MyCommendTypes.NONE)
            {
                RunningTotal = Convert.ToDouble((sender as Button).Text);
                rtbCalc.Text = RunningTotal.ToString();
            }
            else
            {

                RPC_Marsheller.RPCObject data = RPC_Marsheller.RPCObject.Pack<double[]>
                    (Command.ToString(), new double[2] { RunningTotal, Convert.ToDouble((sender as Button).Text) });
                RunningTotal = Calculate(data);
                rtbCalc.Text = RunningTotal.ToString();
                Command = MyCommendTypes.NONE;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Command = MyCommendTypes.ADD;
        }

        private void btnPow_Click(object sender, EventArgs e)
        {
            Command = MyCommendTypes.POW;
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            Command = MyCommendTypes.DIV;
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            Command = MyCommendTypes.SUB;
        }

        private void btnMult_Click(object sender, EventArgs e)
        {
            Command = MyCommendTypes.MULT;
        }
    }
}
