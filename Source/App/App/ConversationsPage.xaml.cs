using System;
using System.Collections.Generic;
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
		}

		async void OnItemAdded(object sender, EventArgs e)
		{
		}
	}
}
