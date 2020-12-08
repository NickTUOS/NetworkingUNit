using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RPC_Marsheller
{
    [Serializable]
    public class RPCObject
    {
        public byte[] data;
        public string RemoteMethordName;

        public RPCObject(byte[] Data, string MethordName)
        {
            data = Data;
            RemoteMethordName = MethordName;
        }


        static public RPCObject Pack<T>(string methodName, T Data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, Data);
                return new RPCObject(ms.ToArray(), methodName);
            }

        }
        static public T Unpack<T>(byte[] Data)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(Data, 0, Data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                T obj = (T)binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
