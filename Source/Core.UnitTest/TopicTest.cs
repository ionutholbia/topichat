using NUnit.Framework;
using System;

namespace Topichat.Core.UnitTest
{
    [TestFixture]
    public class TopicTest
    {
		static readonly Contact[] dummyContacts = new[] {
			new Contact { FirstName = "Me", LastName = "User", PhoneNumber = "111" },
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

		Topic topic;

        [SetUp]
        public void SetUp()
        {
            topic = new Topic(dummyContacts, "id1", "test");
        }

        [Test]
        public void Topic_MessageFromKnownParticipant()
        {
            var participantsChanged = false;
            topic.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Participants")
                {
                    participantsChanged = true;
                }
            };

            var message =
                new Message { TimeStamp = DateTime.UtcNow.AddMonths(-1), Sender = me, Text = "Salut ce faci?", Topic = "Default" };
            topic.Add(message);

            Assert.IsFalse(participantsChanged);
		}

		[Test]
		public void Topic_MessageFromNewParticipant()
		{
			var participantsChanged = false;
			topic.PropertyChanged += (sender, e) => { participantsChanged = true; };

			var cosmin = new Contact
			{
				FirstName = "Cosmin",
				LastName = "Ionescu",
				PhoneNumber = "3435335345"
			};

			var message =
                new Message { TimeStamp = DateTime.UtcNow.AddMonths(-1), Sender = cosmin, Text = "Mersi de invitatie!", Topic = "Default" };
			topic.Add(message);

            Assert.IsTrue(participantsChanged);
		}
	}
}
