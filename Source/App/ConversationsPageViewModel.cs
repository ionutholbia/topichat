using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class ConversationsPageViewModel
    {
        readonly INavigation navigation;
        readonly ConversationsPage masterDetailPage;

        public ConversationsPageViewModel(INavigation navigation, ConversationsPage conversationPage)
        {
            this.navigation = navigation;
            this.masterDetailPage = conversationPage;
            Conversations = App.ConversationManager?.Conversations;
			SearchCommand = new Command<string>(async (text) => await SearchInContactList(text));
			AddContactCommand = new Command(async () => await OnContactAdded());
		}

        public ObservableCollection<Conversation> Conversations { get; }

		public ICommand AddContactCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }

		async Task SearchInContactList(string text)
		{
		}

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
	}
}
