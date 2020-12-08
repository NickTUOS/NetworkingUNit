using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSystemCommon
{
    public class ChatMessageProcessor
    {
        public delegate void OnLoginRequest(ChatMessage message);
        public delegate void OnLogoutRequest(ChatMessage message);
        public delegate void OnLoginSucessful(ChatMessage message);
        public delegate void OnLoginFailed(ChatMessage message);
        public delegate void OnJoinRoomRequest(ChatMessage message);
        public delegate void OnMessageAll(ChatMessage message);
        public delegate void OnMessageOne(ChatMessage message);
        public delegate void OnChangeNick(ChatMessage message);
        public delegate void OnisAlive(ChatMessage message);
        public delegate void OnStillAlive(ChatMessage message);

        public OnLoginRequest onLoginRequest;
        public OnLogoutRequest onLogoutRequest;
        public OnLoginSucessful onLoginSucessful;
        public OnLoginFailed onLoginFailed;
        public OnJoinRoomRequest onJoinRoomRequest;
        public OnMessageAll onMessageAll;
        public OnMessageOne onMessageOne;
        public OnChangeNick onChangeNick;
        public OnisAlive onisAlive;
        public OnStillAlive onStillAlive;

        public void ProcessChatMessage(ChatMessage message)
        {
            switch(message.TypeOfMessage)
            {
                case ChatMessage.MessageType.LoginRequest:
                    if (onLoginRequest != null)
                        onLoginRequest(message);
                    else
                        throw new Exception("OnLoginRequest is not used.");
                break;

                case ChatMessage.MessageType.LogoutRequest:
                    if (onLogoutRequest != null)
                        onLogoutRequest(message);
                    else
                        throw new Exception("onLogoutRequest is not used.");
                    break;

                case ChatMessage.MessageType.LoginSucessful:
                    if (onLoginSucessful != null)
                        onLoginSucessful(message);
                    else
                        throw new Exception("onLoginSucessful is not used.");
                    break;

                case ChatMessage.MessageType.LoginFailed:
                    
                    if (onLoginFailed != null)
                        onLoginFailed(message);
                    else
                        throw new Exception("onLoginFailed is not used.");
                    break;

                case ChatMessage.MessageType.JoinRoomRequest:
                    
                    if (onJoinRoomRequest != null)
                        onJoinRoomRequest(message);
                    else
                        throw new Exception("onJoinRoomRequest is not used.");
                    break;

                case ChatMessage.MessageType.MessageAll:
                    
                    if (onMessageAll != null)
                        onMessageAll(message);
                    else
                        throw new Exception("onMessageAll is not used.");
                    break;

                case ChatMessage.MessageType.MessageOne:
                    
                    if (onMessageOne != null)
                        onMessageOne(message);
                    else
                        throw new Exception("onMessageOne is not used.");
                    break;

                case ChatMessage.MessageType.ChangeNick:
                    
                    if (onChangeNick != null)
                        onChangeNick(message);
                    else
                        throw new Exception("onChangeNick is not used.");
                    break;

                case ChatMessage.MessageType.isAlive:
                    
                    if (onisAlive != null)
                        onisAlive(message);
                    else
                        throw new Exception("onisAlive is not used.");
                    break;

                case ChatMessage.MessageType.StillAlive:
                    if (onStillAlive != null)
                        onStillAlive(message);
                    else
                        throw new Exception("onStillAlive is not used.");
                    break;

                default:
                    throw (new Exception("this message is not a valid type"));
            }
        }
    }
}
