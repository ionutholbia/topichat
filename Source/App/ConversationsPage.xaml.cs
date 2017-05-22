using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Topichat.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConversationsPage : ContentPage
    {
        public ConversationsPage()
        {
            InitializeComponent();
            BindingContext = new ConversationsPageViewModel();
		}

		void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}

		async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            var conversation = ((ListView)sender).SelectedItem as Conversation;
            if(conversation == null)
            {
                return;
            }

            var mainPage = this.Parent as TabbedPage;
			mainPage.Children[1].BindingContext = new TopicsPageViewModel
			{
                Topics = conversation.Topics
			};

            mainPage.CurrentPage = mainPage.Children[1];
            mainPage.Title = conversation.ParticipantsNames;
		}

		async void OnItemAdded(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new NavigationPage(new ContactsPage()));
		}
	}
}
