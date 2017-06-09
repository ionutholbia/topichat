using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Topichat.Forms
{
    public partial class EditPage : ContentPage
    {
        public EditPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            textEntry?.Focus();
        }
    }
}
