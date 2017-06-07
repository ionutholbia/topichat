using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topichat.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();
            BindingContext = new ContactsPageViewModel();
        }

        public Action<List<Contact>> NewConversationRequest;

		void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
			((ListView)sender).SelectedItem = null;
		}

        List<Contact> SelectedContacts { get; set; } = new List<Contact>();

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = ((ListView)sender).SelectedItem as Contact;
            if (contact == null)
            {
                return;
            }

            contact.Selected = !contact.Selected;

            if (contact.Selected)
            {
                SelectedContacts.Add(contact);
            }
            else
            {
                SelectedContacts.Remove(contact);
            }

            finishToolbarItem.Text = SelectedContacts.Count == 0 ? "Cancel" : "Done";
        }

		async Task OnItemFinish(object sender, EventArgs e)
		{
            await Navigation.PopModalAsync(true);

            if(SelectedContacts.Count == 0)
            {
                return;
            }

            foreach(var contact in SelectedContacts)
            {
                contact.Selected = false;
            }

            NewConversationRequest?.Invoke(SelectedContacts);
		}
	}
}
