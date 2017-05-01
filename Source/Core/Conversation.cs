using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace Topichat.Core
{

	public class Conversation : INotifyPropertyChanged, IEnumerable<Message> 
    {
		public event PropertyChangedEventHandler PropertyChanged;

        public Conversation(string id, string topic)
        {
            participants = new List<Contact>();
            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += OnMessagesChanged;
            Id = id;
            Topic = topic;
        }

        public Conversation(IEnumerable<Contact> otherParties, string topic) : this(Guid.NewGuid().ToString(), topic)
        {
            participants.AddRange(otherParties);
        }

        public Conversation(Contact[] otherParties, string topic) : this((IEnumerable<Contact>)otherParties, topic)
        {
        }

        public string Id { get; private set; }

        public string Topic { get;  set; }

        public IEnumerable<Contact> Participants => this.participants;

		List<Contact> participants;

		public ObservableCollection<Message> Messages { get; private set; }


		void OnMessagesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			var participantsChanged = false;
			foreach (Message msg in e.NewItems) {
				if (!participants.Contains (msg.Sender)) {
					participants.Add (msg.Sender);
					participantsChanged = true;
				}
			}
			if (participantsChanged)
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Participants)));
		}

		// The following members make it so we can initialize a Conversation with the collection initializer
		public void Add (Message msg)
		{
            msg.ConversationId = Id;
            msg.Topic = Topic;
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

