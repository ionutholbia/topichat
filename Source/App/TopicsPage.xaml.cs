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
					Messages = topic.Messages,
                    Participants = topic.Participants.Where(p => p.PhoneNumber != App.ContactManager.Me.PhoneNumber).ToList(),
					TopicId = topic.Id,
					TopicName = topic.Name
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
			var mi = ((MenuItem)sender);
			DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
		}
	}
}
