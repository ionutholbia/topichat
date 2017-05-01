using System;
using System.Collections.ObjectModel;
using System.Linq;
using Topichat.Core;

namespace Topichat.Forms
{
    public class TopicsPageViewModel
    {
        public TopicsPageViewModel()
        {
            Conversations = App.ConversationManager.GetConversations();
        }

        public ObservableCollection<Conversation> Conversations { get; }

    }
}
