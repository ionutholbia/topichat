using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Topichat.Forms;
using ImageCircle.Forms.Plugin.iOS;

namespace App.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();

            LoadApplication(new Topichat.Forms.App());

            return base.FinishedLaunching(app, options);
        }
    }
}
