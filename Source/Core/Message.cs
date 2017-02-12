using System;
using System.Collections.Generic;

namespace Chatopia.Core
{
    public class Message
    {
        public DateTime TimeStamp { get; set; }

        public Contact Sender { get; set; }

        public List<Contact> Receivers { get; set;}

        public string Topic { get; set; }
    
        public string Text { get; set; }

        public string GroupId { get; set; }
    }
}
