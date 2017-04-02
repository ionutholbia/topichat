using System;
using System.Threading.Tasks;

namespace Chatopia.Core
{
    public interface IBrokerConnection
    {
        Action<Message> MessageReceived{ get; set; }

        Task SendMessage(Message message);
    }
}