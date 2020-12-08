using System.Net;
using System;


namespace ChatSystemCommon
{
    public class ChatUser
    {
        public string NickName;
        public string ipAddress;
        public int port;
        public long LastAliveTime;
        public ChatUser(string nickName):this()
        {
            NickName = nickName;
        }
        public ChatUser()
        {
            NickName = "";
            ipAddress = "";
            port = 0;
            LastAliveTime = DateTime.Now.Ticks;
        }
    }
}
