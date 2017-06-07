﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Topichat.Core;
using Topichat.Shared;
using Xamarin.Forms;

namespace App.Droid
{
    [Activity(Label = "App.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

 			LoadApplication(new Topichat.Forms.App(
				new ContactManager(),
				new ConversationManager(
					new BrokerConnection(StorageData.Me.PhoneNumber),
					new StorageData(),
					StorageData.Me)));
		}
    }
}
