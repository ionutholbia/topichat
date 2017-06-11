using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            var chatPageViewModel = BindingContext as ChatPageViewModel;
            if(null == chatPageViewModel)
            {
                throw new NullReferenceException("Binding context is not initialized.");
            }

            chatPageViewModel.ChatTopic.Messages.CollectionChanged += (sender, e) => ScrollListViewToEnd();

            ScrollListViewToEnd();
		}

		public void ScrollListViewToEnd()
		{
			var chatPageViewModel = BindingContext as ChatPageViewModel;
            if(chatPageViewModel == null)
            {
                return;
            }

            if(chatPageViewModel.ChatTopic.Messages.Count < 1)
            {
                return;
            }

            var target = chatPageViewModel.ChatTopic.Messages[chatPageViewModel.ChatTopic.Messages.Count - 1];
			MessagesListView.ScrollTo(target, ScrollToPosition.End, true);
		}

        void MessagesListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }

        void MessagesListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }
	}
}
