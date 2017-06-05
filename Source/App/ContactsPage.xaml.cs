using System;
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

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
			((ListView)sender).SelectedItem = null;
		}

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = ((ListView)sender).SelectedItem as Contact;
            if(contact == null)
            {
                return;
            }

            contact.Selected = !contact.Selected;
            finishToolbarItem.Text = "Done";
        }

		async Task OnItemFinish(object sender, EventArgs e)
		{
            await Navigation.PopModalAsync(true);
		}
	}
}
