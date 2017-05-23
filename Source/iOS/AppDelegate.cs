using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Topichat.Forms;
using ImageCircle.Forms.Plugin.iOS;
using Topichat.Shared;
using Topichat.Core;

namespace App.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();

            LoadApplication(new Topichat.Forms.App(
                new ContactManager(),
                new ConversationManager(
                    new BrokerConnection(StorageData.Me.PhoneNumber),
                    new StorageData(),
                    StorageData.Me)));

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
