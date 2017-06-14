using System;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class EditPageViewModel
    {
        public EditPageViewModel()
        {
			EditCommand = new Command(() =>
			{
                EditFinished?.Invoke(this, Text);
			});
		}

		public string Text { get; set; } = string.Empty;

        public string DefaultText { get; set; } = "Enter text...";
	
        public ICommand EditCommand { get; set; }

        public EventHandler<string> EditFinished;
	}
}
