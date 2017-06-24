﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class ConversationsPageViewModel : INotifyPropertyChanged
    {
        readonly INavigation navigation;
        readonly ConversationsPage masterDetailPage;

        public ConversationsPageViewModel(INavigation navigation, ConversationsPage conversationPage)
        {
            this.navigation = navigation;
            this.masterDetailPage = conversationPage;
            Conversations = BackupConversations = App.ConversationManager?.Conversations;
			AddContactCommand = new Command(async () => await OnContactAdded());
		}

        public ObservableCollection<Conversation> BackupConversations { get; }

        ObservableCollection<Conversation> conversations;
		public ObservableCollection<Conversation> Conversations 
        { 
            get
            {
                return this.conversations;
            }
            private set
            {
                this.conversations = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conversations)));
			}
        }

		public ICommand AddContactCommand { get; private set; }

		async Task OnContactAdded()
		{
			var contactsPage = new ContactsPage();

            contactsPage.NewConversationRequest += async (selectedContacts) => 
            {
                if (App.ConversationManager.FindConversation(selectedContacts) != null)
                {
					await ConversationsPage.PushConversation(
						App.ConversationManager.StartConversation(selectedContacts),
						this.masterDetailPage.Parent as MasterDetailPage);
                    return;
				}

                var editPageContext = new EditPageViewModel
                {
                    DefaultText="Enter Topic Name..."
                };
				editPageContext.EditFinished += async (sender, e) =>
				{
					var topicName = string.IsNullOrEmpty(e) ? "New Topic" : e;
					await this.navigation.PopModalAsync(true);
					await ConversationsPage.PushConversation(
						App.ConversationManager.StartConversation(selectedContacts),
						this.masterDetailPage.Parent as MasterDetailPage,
						topicName);
				};
				
                await this.navigation.PushModalAsync(new EditPage
				{
                    BindingContext = editPageContext
				}, true);
 			};

			await this.navigation.PushModalAsync(new NavigationPage(contactsPage)
			{
				BarBackgroundColor = (Color)Application.Current.Resources["primaryBlue"],
				BarTextColor = Color.White
			});
		}

		string searchFilter;
		public string SearchFilter
		{
			get
			{
				return searchFilter;
			}
			set
			{
				if (searchFilter != value)
				{
					searchFilter = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchFilter)));
					FilterContacts(searchFilter);
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void FilterContacts(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
                Conversations = BackupConversations;
			}
			else
			{
				text = text.ToLower();
                Conversations = new ObservableCollection<Conversation>(
                    BackupConversations.Where(c => c.Participants.FirstOrDefault(
                        p => p.ToString().ToLower().Contains(text)) != null));
			}
		}
	}
}
