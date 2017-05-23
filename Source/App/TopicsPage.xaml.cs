using System;
using System.Collections.Generic;
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

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var topic = ((ListView)sender).SelectedItem as Topic;
            if (topic == null)
            {
                return;
            }

            var chatPage = new ChatPage
            {
                BindingContext = new ChatPageViewModel
                {
                    Messages = topic.Messages,
                    Participants = topic.Participants as List<Contact>,
                    TopicId = topic.Id,
                    TopicName = topic.Name
                },
                Title = topic.Name,
                BackgroundColor = (Color)Application.Current.Resources["primaryBlue"]
            };

            chatPage.Initialize();
            await Navigation.PushAsync(chatPage);
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
        }
    }
}
