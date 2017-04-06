using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Topichat.Core 
{
    public class StorageData : IStorageData
    {
		static readonly Contact [] dummyContacts = new[] {
			new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
			new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			new Contact { FirstName = "Andrei", LastName = "Ionescu", PhoneNumber = "0040744222222" },
			new Contact { FirstName = "Paul", LastName = "Matei", PhoneNumber = "0040744333333" },
			new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "0040744444444" },
		};

        static readonly Contact me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "111"
        };
        		
        static readonly Conversation [] dummyConversations = new[] {
            new Conversation("id1") {
                new Message {
                    TimeStamp = DateTime.UtcNow.AddMonths (-1),
                    Sender = dummyContacts [0],
                    Text = "Hey!", Topic = "Default"},
                new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Salut ce faci?", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "Salut!", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [0], Text = "Hey", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "nimic interesant...", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [1], Text = "ok", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "This is a really long message to test that the preview and message bubbles wrap as expected. Hooray!", Topic = "Default"}
			},
            new Conversation("id2") {
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = dummyContacts [1], Text = "Sal!", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Esti?", Topic = "Default"},
			}
		};

        public async Task GetConversations (ObservableCollection<Conversation> conversations)
		{
			foreach (var convo in dummyConversations) {
				conversations.Add (convo);
			}
		}
    }
}

