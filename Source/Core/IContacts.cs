using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Topichat
{
    public interface IContacts
    {
        Contact Me { get; }

        ObservableCollection<Contact> GetContacts();
    }
}
