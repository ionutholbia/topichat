using System;
using System.Collections.Generic;
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
        readonly IBrokerConnection brokerConnection;

        public ConversationManager(IBrokerConnection brokerManager, IStorageData storageData, Contact me)
        {
            this.brokerConnection = brokerManager;
            this.storageData = storageData;
            this.me = me;

            this.brokerConnection.MessageReceived += BrokerConnectionMessageReceived;
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

        public async Task<Conversation> StartConversation(List<Contact> contacts, string topic)
        {
            var conversation = conversations.FirstOrDefault(
                c => c.Participants.Where(p => p != this.me).SequenceEqual(contacts) && c.Topic == topic);
            return conversation ?? NewConversation(contacts, topic);
        }

        void BrokerConnectionMessageReceived(Message message)
        {
            var conversation = conversations.FirstOrDefault(c => c.Id == message.ConversationId);
            if (conversation == null)
            {
                conversation = NewConversation(message.Receivers, message.Topic);
            }

            conversation.Add(message);
        }

        Conversation NewConversation(List<Contact> contacts, string topic)
        {
            var conversation = new Conversation(contacts, topic);
            conversations.Add(conversation);
            return conversation;
        }
    }
}
