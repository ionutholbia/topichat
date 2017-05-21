using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Topichat.Core {

	public interface IConversationManager 
    {
        ObservableCollection<Conversation> GetConversations ();

		Task<Topic> StartConversation (List<Contact> contacts, string topic); 
	}
}

