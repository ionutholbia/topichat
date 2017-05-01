using Xamarin.Forms;

namespace Topichat.Forms
{
    public partial class ConversationsPage : ContentPage
    {
        public ConversationsPage()
        {
            InitializeComponent();
            BindingContext = new ConversationsPageViewModel();

            Navigation.PushModalAsync(new NavigationPage(new ContactsPage()));
        }
    }
}
