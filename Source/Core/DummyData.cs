using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Chatopia.Core 
{
    public class DummyData
    {
		static readonly Contact me = new Contact {
			FirstName = "Me",
			LastName = "User"
		};

		static readonly Contact [] dummyContacts = new[] {
			new Contact { FirstName = "Kate", LastName = "Bell" },
			new Contact { FirstName = "Johnny", LastName = "Appleseed" },
			new Contact { FirstName = "John", LastName = "Doe" },
			new Contact { FirstName = "Steve", LastName = "Jobs" },
			new Contact { FirstName = "Bill", LastName = "Gates" },
		};

		static readonly Conversation [] dummyConversations = new[] {
            new Conversation {
                new Message {
                    TimeStamp = DateTime.UtcNow.AddMonths (-1),
                    Sender = dummyContacts [0],
                    Text = "Hey!"},
                new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Hey what's up?"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "Hey again!"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [0], Text = "Sup"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "Not much"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [1], Text = "Cool"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = me, Text = "This is a really long message to test that the preview and message bubbles wrap as expected. Hooray!"}
			},
			new Conversation {
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = dummyContacts [1], Text = "Hey!"},
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Hey what's up?"},
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

