using System;
using System.Collections.ObjectModel;
using Chatopia;

namespace Chatopia.Ios
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
