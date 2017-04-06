using System;

namespace Topichat.Core
{
    public interface IBrokerConnection : IDisposable
    {
        Action<Message> MessageReceived { get; set;}

    }
}