using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class ContactsPageViewModel : INotifyPropertyChanged
    {
        public ContactsPageViewModel()
        {
            Contacts = ContactsBackup = App.ContactManager.Contacts;
        }

		ObservableCollection<Contact> ContactsBackup { get; }

        ObservableCollection<Contact> contact;
		public ObservableCollection<Contact> Contacts
		{
			get { return contact; }
			set
			{
				contact = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Contacts)));
            }
        }

        string searchFilter;
        public string SearchFilter
		{
			get
			{
				return searchFilter;
			}
			set
			{
				if (searchFilter != value)
				{
					searchFilter = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchFilter)));
					FilterContacts(searchFilter);
				}
			}
		}

        public event PropertyChangedEventHandler PropertyChanged;

        void FilterContacts(string text)
        {
            if (string.IsNullOrEmpty(text))
			{
                Contacts = ContactsBackup;
			}
			else
			{
				text = text.ToLower();
                Contacts = new ObservableCollection<Contact>(ContactsBackup.Where(c => c.ToString().ToLower().Contains(text.ToLower())));
			}
        }
    }
}
