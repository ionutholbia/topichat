using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public class ConversationManager : IConversationManager
    {
        ObservableCollection<Conversation> conversations;

        readonly IStorageData storageData;
        readonly IBrokerConnection brokerConnection;
        readonly Contact me;

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

            foreach(var conv in conversations)
            {
                conv.PropertyChanged += ReorderConversations;
            }

            return conversations;
        }

        void ReorderConversations(object sender, PropertyChangedEventArgs e)
        {
            var topic = sender as Topic;
            var conversation = this.conversations.SingleOrDefault(conv => conv.Topics.SingleOrDefault(arg => arg.Id == topic.Id) != null);
            conversation.Topics.Remove(topic);

			conversation = conversations.FirstOrDefault(
                c => c.Participants.Where(p => p != this.me).SequenceEqual(topic.Participants));

            if(conversation == null)
            {
                conversation = NewConversation(topic.Participants as List<Contact>);
            }
            conversation.Add(topic);
		}

        public async Task<Topic> StartConversation(List<Contact> contacts, string topic)
        {
            var conversation = conversations.FirstOrDefault(
                c => c.Participants.Where(p => p != this.me).SequenceEqual(contacts));

            return conversation.StartTopic(Guid.NewGuid().ToString(), topic) ?? 
                               NewConversation(contacts).StartTopic(Guid.NewGuid().ToString(), topic);
        }

        Conversation NewConversation(List<Contact> contacts)
        {
            var conversation = new Conversation(contacts, me);
			conversation.PropertyChanged += ReorderConversations;
			
            this.conversations.Add(conversation);

            return conversation;
        }

        void BrokerConnectionMessageReceived(Message message)
        {
            var topic = this.conversations.SelectMany(conv => conv.Topics)
                            .FirstOrDefault(top => top.Id == message.TopicId);
            if (topic == null)
            {
                message.Receivers.Add(message.Sender);
                var conversation = NewConversation(message.Receivers);
                this.conversations.Add(conversation);

                topic = conversation.StartTopic(message.TopicId, message.Topic);
            }

            topic.Add(message);
        }
    }
}
