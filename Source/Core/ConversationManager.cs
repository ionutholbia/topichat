﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public class ConversationManager : IConversationManager
    {
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

		ObservableCollection<Conversation> conv;
		public ObservableCollection<Conversation> Conversations
		{
			get
			{
				if (conv == null)
				{
					conv = new ObservableCollection<Conversation>();
					this.storageData.GetConversations(conv);
				}

				foreach (var conversation in conv)
				{
					conversation.PropertyChanged += ReorderConversations;
				}

				return conv;
			}
		}

        public async Task SendMessage(Message message)
        {
            await this.brokerConnection.SendMessage(message);
        }

        public Conversation StartConversation(List<Contact> contacts)
		{
			var conversation = Conversations.FirstOrDefault(
                c => c.Participants.Where(p => p.PhoneNumber != this.me.PhoneNumber).
                SequenceEqual(contacts, new ContactComparer()));

			return conversation ?? NewConversation(contacts);
		}
		
        void ReorderConversations(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != "Participants")
            {
                return;    
            }

            var topic = sender as Topic;
            var conversation = Conversations.SingleOrDefault(conv => conv.Topics.SingleOrDefault(arg => arg.Id == topic.Id) != null);
            conversation.Topics.Remove(topic);

			conversation = Conversations.FirstOrDefault(
                c => c.Participants.Where(p => p != this.me).SequenceEqual(topic.Participants));

            if(conversation == null)
            {
                conversation = NewConversation(topic.Participants as List<Contact>);
            }
            conversation.Add(topic);
		}

        Conversation NewConversation(List<Contact> contacts)
        {
            var conversation = new Conversation(contacts, me);
 			conversation.PropertyChanged += ReorderConversations;
			
            Conversations.Insert(0, conversation);

            return conversation;
        }

        void BrokerConnectionMessageReceived(Message message)
        {
            var topic = Conversations.SelectMany(conv => conv.Topics)
                            .FirstOrDefault(top => top.Id == message.TopicId);
            if (topic == null)
            {
                message.Receivers.Add(message.Sender);
                var conversation = NewConversation(message.Receivers);
                Conversations.Insert(0, conversation);

                topic = conversation.StartTopic(message.TopicId, message.Topic);
            }

            topic.Add(message);
        }
    }
}
