using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Topichat.Core {

	public interface IConversationManager 
    {
        ObservableCollection<Conversation> Conversations { get; }

		Conversation StartConversation (List<Contact> contacts);

        Conversation FindConversation(List<Contact> participants);
        		
        Topic FindTopic(List<Contact> participants, string topicId);

        Task ConnectToBroker();

		Task SendMessage(Message message);
	}
}

