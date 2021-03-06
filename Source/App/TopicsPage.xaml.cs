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
    public partial class TopicsPage : ContentPage
    {
        public TopicsPage()
        {
            InitializeComponent();
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public async Task StartChat(Topic topic)
        {
			var chatPage = new ChatPage
			{
                BindingContext = new ChatPageViewModel(Navigation)
				{
                    ChatTopic = topic
				},
				Title = topic.Name,
				BackgroundColor = (Color)Application.Current.Resources["primaryBlue"]
			};

			chatPage.Initialize();
			await Navigation.PushAsync(chatPage);
		}

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var topic = ((ListView)sender).SelectedItem as Topic;
            if (topic == null)
            {
                return;
            }

            await StartChat(topic);
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            var editPageContext = new EditPageViewModel
            {
                DefaultText = "Enter Topic Name..."    
            };

			editPageContext.EditFinished += async (s, ev) =>
			{
                var topicName = string.IsNullOrEmpty(ev) ? "New Topic" : ev;
                await Navigation.PopModalAsync(true);

				var bindingContex = BindingContext as TopicsPageViewModel;
				var conversation = App.ConversationManager.StartConversation(bindingContex.Participants);
                await StartChat(conversation.StartTopic(Guid.NewGuid().ToString(), topicName));
			};

			await Navigation.PushModalAsync(new EditPage
			{
				BindingContext = editPageContext
			}, true);
		}
	
        public void OnDelete(object sender, EventArgs e)
		{
            var menuItem = ((MenuItem)sender);
			
            var bindingContex = BindingContext as TopicsPageViewModel;
			var conversation = App.ConversationManager.FindConversation(bindingContex.Participants);
            if(conversation == null)
            {
				DisplayAlert("Topichat", "Error deleting topic!", "OK");
                return;
			}
			
            conversation.DeleteTopic(menuItem.CommandParameter as Topic);
		}
	}
}
