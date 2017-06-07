using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace Topichat.Core
{

	public class Topic : INotifyPropertyChanged, IEnumerable<Message> 
    {
		public event PropertyChangedEventHandler PropertyChanged;

        public Topic(IEnumerable<Contact> otherParties, string id, string topic)
        {
			participants = new List<Contact>();
			Messages = new ObservableCollection<Message>();
			Messages.CollectionChanged += OnMessagesChanged;
			Id = id;
			Name = topic;
			
            participants.AddRange(otherParties);
        }

        public string Id { get; private set; }

        public string Name { get;  set; }

        public IEnumerable<Contact> Participants => this.participants;

		List<Contact> participants;

		public ObservableCollection<Message> Messages { get; private set; }

        public Message LastMessage => Messages?.LastOrDefault() ?? new Message
        {
            Text = "...",
            TimeStamp = DateTime.MinValue
        };

		void OnMessagesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			var participantsChanged = false;
			foreach (Message msg in e.NewItems) {
                if (participants.SingleOrDefault(part => part.PhoneNumber == msg.Sender.PhoneNumber) == null) 
                {
					participants.Add (msg.Sender);
					participantsChanged = true;
				}
			}
			if (participantsChanged)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Participants)));    
            }
		}

		// The following members make it so we can initialize a Conversation with the collection initializer
		public void Add (Message msg)
		{
            msg.TopicId = Id;
            msg.Topic = Name;
			Messages.Add (msg);
		}

		public IEnumerator<Message> GetEnumerator ()
		{
			return Messages.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}

