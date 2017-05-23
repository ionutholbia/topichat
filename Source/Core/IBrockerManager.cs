using System;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public interface IBrokerConnection : IDisposable
    {
        Action<Message> MessageReceived { get; set;}

        Task SendMessage(Message message);
	}
}