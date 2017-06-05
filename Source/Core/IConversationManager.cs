using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Topichat.Core {

	public interface IConversationManager 
    {
        ObservableCollection<Conversation> GetConversations ();

        Task<Conversation> StartConversation (List<Contact> contacts);

        Task SendMessage(Message message);
	}
}

