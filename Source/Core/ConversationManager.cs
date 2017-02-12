using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace Chatopia.Core
{
    public class ConversationManager : IConversationManager
    {
        public ObservableCollection<Conversation> GetConversations()
        {
            throw new NotImplementedException();
        }

        public Task<Conversation> StartConversation(params Contact[] contacts)
        {
            throw new NotImplementedException();
        }
    }
}
