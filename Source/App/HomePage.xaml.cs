using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Topichat.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Topichat.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();
		}          
    }
}
