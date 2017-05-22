using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Topichat.Forms;
using Topichat.Core;
using Topichat.Shared;

namespace Topichat.App.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Topichat.Forms.App(new ContactManager(),
                new ConversationManager(
                    new BrokerConnection("00400744360800"),
                    new StorageData(),
                    new Contact { FirstName = "Ionut", LastName = "Holbia", PhoneNumber = "00400744360800" })));

        }
    }
}
