using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Globalization;
using Topichat.Core;
using MvvmHelpers;

namespace Topichat.Forms
{
    public class ChatPageViewModel : BaseViewModel
    {
        public Topic ChatTopic{ get; set; }

        readonly INavigation navigation;

		string outgoingText = string.Empty;
        public string OutGoingText
		{
			get { return outgoingText; }
			set { SetProperty(ref outgoingText, value); }
		}

		public ICommand SendCommand { get; set; }
        		
        public ICommand LocationCommand { get; set; }

        public ChatPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;

            SendCommand = new Command(async () => await SendMessage(OutGoingText));
        }

        async Task SendMessage(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return;
            }

            var message = new Message
            {
                Text = text,
                Sender = App.ContactManager.Me,
                Receivers = ChatTopic.Participants.Where(p => p.PhoneNumber != App.ContactManager.Me.PhoneNumber).ToList(),
                TimeStamp = DateTime.Now,
                Topic = ChatTopic.Name,
                TopicId = ChatTopic.Id
            };

            await App.ConversationManager.SendMessage(message);
            ChatTopic.Add(message);
            OutGoingText = string.Empty;
        }            
	}
    
}
