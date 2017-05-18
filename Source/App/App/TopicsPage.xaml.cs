using System;
using Topichat.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopicsPage : ContentPage
    {
        readonly TopicsPageViewModel conversationViewPageModel = new TopicsPageViewModel();

        public TopicsPage()
        {
            InitializeComponent();
            BindingContext = this.conversationViewPageModel;

            //Navigation.PushModalAsync(new NavigationPage(new ContactsPage()));
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
        	((ListView)sender).SelectedItem = null;
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        	var contact = ((ListView)sender).SelectedItem as Conversation;
        }

		async void OnItemAdded(object sender, EventArgs e)
		{
		}
	}
}
