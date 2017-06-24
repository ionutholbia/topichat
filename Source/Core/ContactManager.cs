using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Topichat;

namespace Topichat.Core
{
    public class ContactManager : IContacts
    {

		static readonly Contact[] dummyContacts = new[] {
            new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "0040744000000" },
            new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "999" },
            new Contact { FirstName = "Andrei", LastName = "Ionescu", PhoneNumber = "0040744222222" },
            new Contact { FirstName = "Paul", LastName = "Matei", PhoneNumber = "0040744333333" },
			new Contact { FirstName = "Vasile", LastName = "Ionescu", PhoneNumber = "222" },
			new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "0040744444444" }
        };

        readonly Contact me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "111"
        };

        readonly Contact unknownContact = new Contact
        {
            FirstName = "Unknown"
        };

        readonly ObservableCollection<Contact> contacts;
		
        public ContactManager()
        {
			this.contacts = new ObservableCollection<Contact>();
			foreach (var contact in dummyContacts)
			{
				contacts.Add(contact);
			}
		}

        public Contact Me => this.me;

        public ObservableCollection<Contact> Contacts => this.contacts;

        public Contact FindContact(string phoneNumber)
        {
            if(Me.PhoneNumber == phoneNumber)
            {
                return Me;
            }

            unknownContact.PhoneNumber = phoneNumber;
            return this.contacts.FirstOrDefault(c => c.PhoneNumber == phoneNumber) ?? unknownContact;
        }
    }
}
