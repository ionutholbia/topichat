using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Topichat.Core;
using Xamarin.Forms;

namespace Topichat.Forms
{
    public class TopicsPageViewModel : INotifyPropertyChanged
    {
        public TopicsPageViewModel(ObservableCollection<Topic> topics, List<Contact> participants)
        {
            BackupTopics = Topics = topics;
            Participants = participants;
        }

        ObservableCollection<Topic> BackupTopics { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<Topic> topics;
        public ObservableCollection<Topic> Topics
        {
			get { return topics; }
			set
			{
				topics = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Topics)));
			}
		}

        public List<Contact> Participants { get; private set; }


		string searchFilter;
		public string SearchFilter
		{
			get
			{
				return searchFilter;
			}
			set
			{
				if (searchFilter != value)
				{
					searchFilter = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchFilter)));
					FilterContacts(searchFilter);
				}
			}
		}

        void FilterContacts(string text)
		{
		    if (string.IsNullOrEmpty(text))
		    {
                Topics = BackupTopics;
		    }
		    else
		    {
		        text = text.ToLower();
		        Topics = new ObservableCollection<Topic>(BackupTopics.Where(t => t.Name.ToLower().Contains(text)));
		    }
		}
	}
}
