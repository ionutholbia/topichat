using Xamarin.Forms;
using Topichat.Core;
using System.Threading.Tasks;

namespace Topichat.Forms
{
    public class App : Application
    {
        internal static IContacts ContactManager { get; private set; }

        internal static IConversationManager ConversationManager { get; private set; }

        public App(IContacts contactManager, IConversationManager conversationManager)
        {
            ConversationManager = conversationManager;
            ContactManager = contactManager;

            InitApp();
        }

        public App()
        {
            InitApp();
        }

        void InitApp()
        {
            Resources = new ResourceDictionary
            {
                { "primaryBlue", Color.FromHex("509ee2") }
            };

            MainPage = new HomePage
            {
                BackgroundColor = (Color)Application.Current.Resources["primaryBlue"],
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Task.Run(async () => await ConversationManager.ConnectToBroker());
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
