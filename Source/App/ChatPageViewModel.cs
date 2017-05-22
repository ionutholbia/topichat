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

namespace Topichat.Forms
{
    public class ChatPageViewModel
    {
        public ObservableCollection<Message> Messages { get; set; }

        public string OutGoingText { get; set; }

        public ICommand SendCommand { get; set; }

        public ICommand LocationCommand { get; set; }

        public ChatPageViewModel()
        {
            SendCommand = new Command<string>(async (text) => await SendMessage(text));
        }

		private async Task SendMessage(string text)
        {
            throw new NotImplementedException();
        }            
	}
    
}
