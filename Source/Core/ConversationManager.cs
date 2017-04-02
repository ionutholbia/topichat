using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace Topichat.Core
{
    public class ConversationManager : IConversationManager, IContacts
    {
        ObservableCollection<Contact> contacts;
        ObservableCollection<Conversation> conversations;
        IBrokerConnection brokerConnection;

        public ConversationManager(IBrokerConnection brokerConnection)
        {
            this.brokerConnection = brokerConnection;
        }

        public Contact Me => DummyData.Me;

        public ObservableCollection<Contact> GetContacts()
        {
            if (contacts == null)
            {
                contacts = new ObservableCollection<Contact>();
                DummyData.GetContacts(contacts);
            }

            return contacts;
        }

        public ObservableCollection<Conversation> GetConversations()
        {
            if (conversations == null)
            {
                conversations = new ObservableCollection<Conversation>();
                DummyData.GetConversations(conversations);
            }

            return conversations;
        }

        public async Task<Conversation> StartConversation(params Contact[] contacts)
        {
            await Task.Delay(500);
            var convo = conversations.FirstOrDefault(c => c.Participants.Where(p => p != Me).SequenceEqual(contacts));
            if (convo == null)
            {
                convo = new Conversation(this.brokerConnection, contacts);
                conversations.Add(convo);
            }
            return convo;
        }
    }
}
