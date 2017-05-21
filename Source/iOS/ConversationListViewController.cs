using System;
using System.Linq;
using System.Collections.ObjectModel;

using UIKit;
using Foundation;
using Topichat.Core;

namespace Topichat.Ios 
{
    public partial class ConversationListViewController : UITableViewController {

		public ConversationListViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 44;

			TableView.DataSource = new BindingTableDataSource (TableView, GetCell) {
				Binding = App.conversationManager.GetConversations ()
			};
		}

		protected virtual UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath, object data)
		{
			var cell = (ConversationCell)tableView.DequeueReusableCell (ConversationCell.Id, indexPath);
			cell.Conversation = (Topic)data;
			return cell;
		}
		
		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
			if (segue.Identifier == "Edit") {
				var dest = (ConversationViewController)segue.DestinationViewController;
				dest.Conversation = ((ConversationCell)sender).Conversation;
			}
		}
	}
}