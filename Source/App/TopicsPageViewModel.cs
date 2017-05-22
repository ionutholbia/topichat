﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class TopicsPageViewModel
    {
        public TopicsPageViewModel()
        {
            SearchCommand = new Command<string>(async(text) => await SearchInContactList(text));
        }

        public ObservableCollection<Topic> Topics { get; set; }

        public ICommand SearchCommand { get; private set; }

        async Task SearchInContactList(string text)
        {
        }
    }
}
