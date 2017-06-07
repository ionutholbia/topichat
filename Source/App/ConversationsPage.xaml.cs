using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConversationsPage : ContentPage
    {
        readonly ConversationsPageViewModel conversationsPageViewModel;

        public ConversationsPage()
        {
            InitializeComponent();
            conversationsPageViewModel = new ConversationsPageViewModel(Navigation, this);
            BindingContext = conversationsPageViewModel;
		}

		void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}

		protected override async void OnAppearing()
        {
            if(this.conversationsListView.SelectedItem == null)
            {
                this.conversationsListView.SelectedItem = this.conversationsPageViewModel.Conversations.FirstOrDefault();
            }

            if (this.Parent is MasterDetailPage masterDetailPage)
            {
                masterDetailPage.IsPresented = true;
            }
        }

        public static async Task PushConversation(Conversation conversation, MasterDetailPage masterDetailPage)
        {
            var topicsPage = new TopicsPage
            {
                BindingContext = new TopicsPageViewModel
                {
                    Topics = conversation.Topics,
                    Participants = conversation.Participants.ToList()
                },
                Title = conversation.ParticipantsNames
            };
            masterDetailPage.Detail = new NavigationPage(topicsPage)
			{
				BarBackgroundColor = (Color)Application.Current.Resources["primaryBlue"],
				BarTextColor = Color.White
			};

            if(conversation.Topics.Count == 0)
            {
                await topicsPage.StartChat(conversation.StartTopic(Guid.NewGuid().ToString(), "New Topic"));
            }

			masterDetailPage.IsPresented = false;
		}

		async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            var conversation = ((ListView)sender).SelectedItem as Conversation;
            if(conversation == null)
            {
                return;
            }

            await PushConversation(conversation, this.Parent as MasterDetailPage);
		}

		public void OnDelete(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
		}
	}
}
