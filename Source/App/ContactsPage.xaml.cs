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
        }
    }
}
