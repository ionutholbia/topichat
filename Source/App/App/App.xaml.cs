using Xamarin.Forms;
using Topichat.Core;

namespace App
{
    public partial class App : Application
    {
        internal static ContactManager contactManager = new ContactManager();

        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
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
