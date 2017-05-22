using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    class ChatPageTemplateSelector : DataTemplateSelector
    {
        public ChatPageTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(ChatIncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(ChatOutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as Message;
            if (message == null)
                return null;
            
            return message.Sender.PhoneNumber == App.ContactManager.Me.PhoneNumber ? 
                            this.outgoingDataTemplate : this.incomingDataTemplate;
        }

        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;
    }
}
