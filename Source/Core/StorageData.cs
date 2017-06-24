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

        public static readonly Contact Me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "111"
        };
        		
        static readonly Topic [] dummyConversations = new[] {
            new Topic(new Contact [] 
            {
				new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
				new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			}, "id1", "Vacanta la munte. Primul meu topic folosind Topichat") 
            {
                new Message {
                    TimeStamp = DateTime.UtcNow.AddMonths (-1),
                    Sender = dummyContacts [0],
                    Text = "Hey!", Topic = "Default"},
                new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = Me, Text = "Salut ce faci?", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = Me, Text = "Salut!", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [0], Text = "Hey", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = Me, Text = "nimic interesant...", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = dummyContacts [1], Text = "ok", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMinutes (-1), Sender = Me, Text = "Acesta este un mesaj foarte lung folosit pentru a testa daca se afiseaza corect in aplicatiile noastre.", Topic = "Default"}
			},
			new Topic(new Contact []
			{
				new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			},"id2", "Proiect facultate") {
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = dummyContacts [1], Text = "Sal!", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = Me, Text = "Esti?", Topic = "Default"},
			}
		};

        public async Task GetConversations (ObservableCollection<Conversation> conversations)
		{
            conversations.Add(new Conversation(new Contact[]
            {
                new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
                new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
            }, Me)
            {
                dummyConversations[0]
            });

            conversations.Add(new Conversation(new Contact[]
			{
				new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			}, Me)
			{
				dummyConversations[1]
			});
		}
    }
}

