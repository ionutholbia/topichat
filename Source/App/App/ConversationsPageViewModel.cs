﻿using System;
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
        public ConversationsPageViewModel()
        {
			Conversations = App.ConversationManager.GetConversations();
			SearchCommand = new Command<string>(async (text) => await SearchInContactList(text));
		}

        public ObservableCollection<Conversation> Conversations { get; }

		public ICommand SearchCommand { get; private set; }

		async Task SearchInContactList(string text)
		{
		}
	}
}
