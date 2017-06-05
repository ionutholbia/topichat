using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;
namespace Topichat.Forms
{
    public class ConversationsPageViewModel
    {
        readonly INavigation navigation;

        public ConversationsPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;
			Conversations = App.ConversationManager?.GetConversations();
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
            await this.navigation.PushModalAsync(new NavigationPage(new ContactsPage())
            {
				BarBackgroundColor = (Color)Application.Current.Resources["primaryBlue"],
				BarTextColor = Color.White
			});
		}
	}
}
