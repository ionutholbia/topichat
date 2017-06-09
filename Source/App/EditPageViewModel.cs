using System;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class EditPageViewModel
    {
        public EditPageViewModel(string defaultText)
        {
            Text = defaultText;

			EditCommand = new Command(() =>
			{
                EditFinished?.Invoke(this, Text);
			});
		}

        public string Text { get; set; }
	
        public ICommand EditCommand { get; set; }

        public EventHandler<string> EditFinished;
	}
}
