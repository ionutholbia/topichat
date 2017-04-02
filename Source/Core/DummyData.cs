using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Topichat.Core 
{
    public class DummyData
    {
        static readonly Contact me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "+40744360800"
		};

		static readonly Contact [] dummyContacts = new[] {
			new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "+40744000000" },
			new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "+40744111111" },
			new Contact { FirstName = "Andrei", LastName = "Ionescu", PhoneNumber = "+40744222222" },
			new Contact { FirstName = "Paul", LastName = "Matei", PhoneNumber = "+40744333333" },
			new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "+40744444444" },
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
			},
		};

		public static Contact Me => me;

        public static async Task GetContacts (ObservableCollection<Contact> contacts)
		{
			foreach (var contact in dummyContacts) {
				await Task.Delay (500);
				contacts.Add (contact);
			}
		}

        public static async Task GetConversations (ObservableCollection<Conversation> conversations)
		{
			foreach (var convo in dummyConversations) {
				await Task.Delay (500);
				conversations.Add (convo);
			}
		}
	}
}

