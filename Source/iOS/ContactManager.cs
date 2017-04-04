using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Topichat;

namespace Topichat.Ios
{
    public class ContactManager : IContacts
    {
        static readonly Contact[] dummyContacts = new[] {
            new Contact { FirstName = "Olesea", LastName = "Holbia", PhoneNumber = "+40744000000" },
            new Contact { FirstName = "Luca", LastName = "Stefan", PhoneNumber = "+40744111111" },
            new Contact { FirstName = "Andrei", LastName = "Ionescu", PhoneNumber = "+40744222222" },
            new Contact { FirstName = "Paul", LastName = "Matei", PhoneNumber = "+40744333333" },
            new Contact { FirstName = "Adrian", LastName = "Maxim", PhoneNumber = "+40744444444" }
        };

        readonly Contact me = new Contact
        {
            FirstName = "Me",
            LastName = "User",
            PhoneNumber = "0040744360800"
        };

        public Contact Me => this.me;

        public ObservableCollection<Contact> GetContacts()
        {
            var contacts = new ObservableCollection<Contact>();
            GetContacts(contacts);
            return contacts;
        }

        async Task GetContacts(ObservableCollection<Contact> contacts)
        {
            foreach (var contact in dummyContacts)
            {
                await Task.Delay(500);
                contacts.Add(contact);
            }
        }
    }
}
