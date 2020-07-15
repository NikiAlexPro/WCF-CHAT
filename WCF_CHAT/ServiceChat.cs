using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_CHAT
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int id = 1;
        public int Connect(string name)
        {
            ServerUser serverUser = new ServerUser()
            {
                ID = id,
                Name = name,
                operationContext = OperationContext.Current
            };

            id++;

            Send_Message(": " + serverUser.Name + " Подключился к чату ", 0);
            users.Add(serverUser);

            return serverUser.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if(user != null)
            {
                users.Remove(user);
                Send_Message(": " + user.Name + " Покинул чат ", 0);
            }
        }

        public void Send_Message(string message, int id)
        {
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += " : " + user.Name + " ";
                }
                answer += message;

                item.operationContext.GetCallbackChannel<IServerChatCallBack>().Message_CallBack(answer);
            }
        }
    }
}
