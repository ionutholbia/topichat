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
        public ObservableCollection<Message> Messages { get; set; }

		public List<Contact> Participants { get; set; }

        readonly INavigation navigation;

        string topicName;
        public string TopicName 
        { 
            get
            {
                return this.topicName;
            } 
            set
            {
                SetProperty(ref topicName, value);
                var topic = App.ConversationManager?.FindTopic(Participants, TopicId)?.Name;
                if(topic != null && topic != this.topicName)
                {
                    topic = this.topicName;
                }
            } 
        } 

        public string TopicId { get; set; }

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
                Receivers = Participants,
                TimeStamp = DateTime.Now,
                Topic = TopicName,
                TopicId = TopicId
            };

            await App.ConversationManager.SendMessage(message);
            Messages.Add(message);
            OutGoingText = string.Empty;
        }            
	}
    
}
