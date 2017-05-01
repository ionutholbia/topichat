using Xamarin.Forms;
using Topichat.Core;

namespace Topichat.Forms
{
    public partial class App : Application
    {
        internal static IContacts ContactManager { get; private set; }

        internal static IConversationManager ConversationManager { get; private set; }

        public App(IContacts contactManager, IConversationManager conversationManager)
        {
            ConversationManager = conversationManager;
            ContactManager = contactManager;

            InitializeComponent();

            MainPage = new ConversationsPage();
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
