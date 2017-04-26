using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public interface IContacts
    {
        Contact Me { get; }

        ObservableCollection<Contact> GetContacts();
    }
}
