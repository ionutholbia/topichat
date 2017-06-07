using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Topichat.Core {

	public interface IConversationManager 
    {
        ObservableCollection<Conversation> Conversations { get; }

		Conversation StartConversation (List<Contact> contacts);

        Task SendMessage(Message message);
	}
}

