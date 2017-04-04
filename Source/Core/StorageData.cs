using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Topichat.Core 
{
    public class StorageData : IStorageData
    {
		static readonly Contact [] dummyContacts = new[] {
			new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
			new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "0040744111111" },
			new Contact { FirstName = "Andrei", LastName = "Ionescu", PhoneNumber = "0040744222222" },
			new Contact { FirstName = "Paul", LastName = "Matei", PhoneNumber = "0040744333333" },
			new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "0040744444444" },
		};

        static readonly Contact me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "0040744360800"
        };
        		
        static readonly Conversation [] dummyConversations = new[] {
            new Conversation {
                new Message {
                    TimeStamp = DateTime.UtcNow.AddMonths (-1),
                    Sender = dummyContacts [0],
                    Text = "Hey!"},
                new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Salut ce faci?"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "Salut!"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [0], Text = "Hey"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "nimic interesant..."},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [1], Text = "ok"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "This is a really long message to test that the preview and message bubbles wrap as expected. Hooray!"}
			},
			new Conversation {
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = dummyContacts [1], Text = "Sal!"},
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Esti?"},
			}
		};

        public async Task GetConversations (ObservableCollection<Conversation> conversations)
		{
			foreach (var convo in dummyConversations) {
				await Task.Delay (500);
				conversations.Add (convo);
			}
		}
    }
}

