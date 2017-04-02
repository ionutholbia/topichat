using System;
using System.Collections.ObjectModel;
using Topichat;

namespace Topichat.Ios
{
    public class ContactManager : IContacts
    {
        public ContactManager()
        {
        }

        public Contact Me
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<Contact> GetContacts()
        {
            throw new NotImplementedException();
        }
    }
}
