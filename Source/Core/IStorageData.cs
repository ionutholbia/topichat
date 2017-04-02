using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Topichat.Core
{
    public interface IStorageData
    {
        Task GetConversations(ObservableCollection<Conversation> conversations);
    }
}
