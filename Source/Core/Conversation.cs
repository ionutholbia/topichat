using System;
using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Topichat.Core
{

	public class Conversation : INotifyPropertyChanged, IEnumerable<Message> 
    {
        IBrokerConnection brokerConnection;

		public event PropertyChangedEventHandler PropertyChanged;

        public Conversation()
        {
            participants = new List<Contact>();
            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += OnMessagesChanged;
        }

        public Conversation(IBrokerConnection brokerConnection, IEnumerable<Contact> otherParties) : this()
        {
            this.brokerConnection = brokerConnection;
            participants.AddRange(otherParties);
        }

        public Conversation(IBrokerConnection brokerConnection, params Contact[] otherParties) 
            : this(brokerConnection, (IEnumerable<Contact>)otherParties)
        {
        }

		/// <summary>
		/// Gets or sets the identifier for this <c>Conversation</c>.
		/// </summary>
		/// <remarks>
		/// This property uniquely identifies a specific conversation on the server.
		/// </remarks>
		public string Id 
        {
			get { return id; }
			set {
				if (id != value) 
                {
					id = value;
					PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Id)));
				}
			}
		}
		string id;

        public string Topic { get;  set; }

		public IEnumerable<Contact> Participants 
        {
			get { return participants; }
		}

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

