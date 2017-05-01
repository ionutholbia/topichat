using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class ContactsPageViewModel
    {
        public ContactsPageViewModel()
        {
            Contacts = App.contactManager.GetContacts();
            SearchCommand = new Command<string>(async(text) => await SearchInContactList(text));
        }

        public ObservableCollection<Contact> Contacts { get; }

        public ICommand SearchCommand { get; private set; }

        async Task SearchInContactList(string text)
        {
        }
    }
}
