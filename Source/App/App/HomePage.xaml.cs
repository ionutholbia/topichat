using Xamarin.Forms;

namespace Topichat.Forms
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            Navigation.PushModalAsync(new NavigationPage(new ContactsPage()));
        }
    }
}
