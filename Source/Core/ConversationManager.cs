using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public class ConversationManager : IConversationManager
    {
        ObservableCollection<Conversation> conversations;

        readonly IStorageData storageData;
        readonly Contact me;

        public ConversationManager(IStorageData storageData, Contact me)
        {
            this.storageData = storageData;
            this.me = me;
        }

        public ObservableCollection<Conversation> GetConversations()
        {
            if (conversations == null)
            {
                conversations = new ObservableCollection<Conversation>();
                this.storageData.GetConversations(conversations);
            }

            return conversations;
        }

        public async Task<Conversation> StartConversation(params Contact[] contacts)
        {
            await Task.Delay(500);
            var convo = conversations.FirstOrDefault(c => c.Participants.Where(p => p != this.me).SequenceEqual(contacts));
            if (convo == null)
            {
                convo = new Conversation(contacts);
                conversations.Add(convo);
            }
            return convo;
        }
    }
}
