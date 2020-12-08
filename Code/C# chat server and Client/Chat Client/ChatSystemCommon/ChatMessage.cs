using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ChatSystemCommon
{
    public class ChatMessage
    {
        public enum MessageType
        {
            LoginRequest,
            LogoutRequest,
            LoginSucessful,
            LoginFailed,
            JoinRoomRequest,
            MessageAll,
            MessageOne,
            ChangeNick,
            isAlive,
            StillAlive

        }

        public MessageType TypeOfMessage;
        public ChatUser user;
        public string Room;
        public string Recipiant;
        public string MessageBody;

        public ChatMessage()
        {
            TypeOfMessage = MessageType.MessageAll;
            user = new ChatUser();
            Room = "";
            Recipiant = "";
            MessageBody = "";
    }

        public string SerializeToXML()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChatMessage));
            string xml;

            using (StringWriter sWriter = new StringWriter())
            {
                //using (XmlWriter xmlWriter = XmlWriter.Create(sWriter))
                //{
                    xmlSerializer.Serialize(sWriter, this);
                    xml = sWriter.ToString();
                //}
            }
            return xml;
        }

        public static ChatMessage DeserializeFromXML(string XmlMessage)
        {
            ChatMessage message = null;

            StringReader strReader = null;
            XmlTextReader xmlTextReader = null;
            try
            {
                strReader = new StringReader(XmlMessage);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChatMessage));
                xmlTextReader = new XmlTextReader(strReader);
                message = xmlSerializer.Deserialize(xmlTextReader) as ChatMessage;
            }
            catch(Exception e)
            {
                throw (e);
            }
            finally
            {
                if(xmlTextReader != null)
                {
                    xmlTextReader.Close();
                }
                if(strReader != null)
                {
                    strReader.Close();
                }
            }

            return message;
        }

        public static ChatMessage MakeLoginRequestMassage(ChatUser user)
        {
            ChatMessage logonMessage = new ChatMessage();
            logonMessage.TypeOfMessage = MessageType.LoginRequest;
            logonMessage.user = user;
            return logonMessage;
        }
        public static ChatMessage MakeLoginSucessfulMessage(string messageBody)
        {
            ChatMessage message = new ChatMessage();
            message.TypeOfMessage = MessageType.LoginSucessful;
            message.MessageBody = messageBody;
            return message;
        }
        public static ChatMessage MakeLoginFailedMessage(string messageBody)
        {
            ChatMessage message = new ChatMessage();
            message.TypeOfMessage = MessageType.LoginFailed;
            message.MessageBody = messageBody;
            return message;
        }
        public static ChatMessage MakeisAliveMessage() // are you still there?....
        {
            ChatMessage message = new ChatMessage();
            message.TypeOfMessage = MessageType.isAlive;
            return message;
        }
        public static ChatMessage MakeStillAliveMessage() // for the users who are, still alive.... still alive
        {
            ChatMessage message = new ChatMessage();
            message.TypeOfMessage = MessageType.StillAlive;
            return message;
        }
    }
}
