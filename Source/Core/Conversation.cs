using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public class Conversation : INotifyPropertyChanged, IEnumerable<Topic>
    {
		readonly Contact me;

        public Conversation()
        {
			Topics = new ObservableCollection<Topic>();
		}

        public Conversation(IEnumerable<Contact> participants, Contact me) : this()
        {
            this.me = me;
            Participants = participants;		
		}

        public ObservableCollection<Topic> Topics { get; private set; }

        public IEnumerable<Contact> Participants { get; private set; }
        		
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerator<Topic> GetEnumerator()
        {
            return Topics.GetEnumerator();
		}

        public Topic StartTopic(string id, string name)
		{
            return Topics.SingleOrDefault(top => top.Id == id) ?? NewTopic(id, name);
		}

		public string ImageUrl
		{
			get
			{
                if (Participants.Count() > 1)
				{
					return "https://image.freepik.com/free-icon/group-of-users-silhouette_318-49953.jpg";
				}

				return Participants.FirstOrDefault().ImageUrl;
			}
		}

        public string LastTopic => Topics.FirstOrDefault().Name;

		public string ParticipantsNames
		{
			get
			{
				if (Participants.Count() > 1)
				{
                    return Participants.Select(i => i.FirstName).Aggregate((i, j) => i + "," + j);
				}

				return Participants.FirstOrDefault().FullName;
			}
		}

		// The following members make it so we can initialize a Conversation with the collection initializer
		public void Add(Topic topic)
		{
			topic.PropertyChanged += TopicParticipantsChanged;
			Topics.Add(topic);
		}

        void TopicParticipantsChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        		
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        		
        Topic NewTopic(string id, string name)
		{
            var topic = new Topic(Participants, id, name);
            topic.PropertyChanged += TopicParticipantsChanged;

            this.Topics.Add(topic);

			return topic;
		}
	}
}
