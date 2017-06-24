using System;
using System.Linq;
using NUnit.Framework;

namespace Topichat.Core.UnitTest
{
    [TestFixture]
    public class ConversationTest
    {
		static readonly Contact[] dummyContacts = new[] {
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

		static readonly Topic[] dummyTopics = new[] {
			new Topic(me, new Contact []
			{
				new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
				new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			}, "id1", "Vacanta la munte")
			{
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
			new Topic(me, new Contact []
			{
				new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
				new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
			},"id2", "Proiect facultate") {
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = dummyContacts [1], Text = "Sal!", Topic = "Default"},
				new Message {TimeStamp = DateTime.UtcNow.AddMonths (-1), Sender = me, Text = "Esti?", Topic = "Default"},
			}
		};

        Conversation conversation;

        [Test]
        public void Conversation_NewParticipantAdded()
        {
            conversation = new Conversation
            {
                dummyTopics[0]
            };

            var partChanged = false;
            conversation.PropertyChanged += (sender, e) => { partChanged = true; };

            var adi = new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "0040744444444" };
			var message =
                new Message { TimeStamp = DateTime.UtcNow.AddMonths(-1), Sender = adi, Text = "Mersi de invitatie!", Topic = "Default" };

            conversation.Topics.FirstOrDefault().Add(message);

            Assert.IsTrue(partChanged);
		}

		[Test]
		public void Conversation_NoParticipantAdded()
		{
			conversation = new Conversation
			{
				dummyTopics[0]
			};

			var partChanged = false;
			conversation.PropertyChanged += (sender, e) => { partChanged = true; };

            var olesea = new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" };
			var message =
                new Message { TimeStamp = DateTime.UtcNow.AddMonths(-1), Sender = olesea, Text = "Mersi de invitatie!", Topic = "Default" };

			conversation.Topics.FirstOrDefault().Add(message);

            Assert.IsFalse(partChanged);
		}
	}
}
