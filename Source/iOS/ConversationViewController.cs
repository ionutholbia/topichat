using System;
using System.Linq;
using System.Threading;
using System.Reactive.Linq;
using System.Collections;

using UIKit;
using Foundation;
using Topichat.Core;
using System.Collections.Generic;

namespace Topichat.Ios 
{
	public partial class ConversationViewController : UIViewController, IUITableViewDelegate {

		static readonly NSString contactCellId = new NSString ("ContactCell");

		public Conversation Conversation {
			get { return conversation; }
			set {
				if (conversation != value) {
					conversation = value;
					SetTitleBinding ();
					SetDataBinding ();
				}
			}
		}

		public BindingTableDataSource DataSource => TableView?.DataSource as BindingTableDataSource;

		IDisposable titleBinding;
		NSObject keyboardObserver;
		Conversation conversation;

		public ConversationViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.Delegate = this;
			TableView.DataSource = new BindingTableDataSource (TableView, GetCell);
			SetDataBinding ();

			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 44;

			var ci = TableView.ContentInset;
			ci.Bottom += messageEntry.Frame.Height;
			TableView.ContentInset = ci;

            sendButton.TouchUpInside += SendButtonTouchUpInside;
		}

        void SendButtonTouchUpInside(object sender, EventArgs e)
        {
            var message = new Message 
            { 
                TimeStamp = DateTime.UtcNow,
                Sender = App.contactManager.Me,
                Text = messageText.Text,
                Receivers = Conversation.Participants.Where(p => p.PhoneNumber != App.contactManager.Me.PhoneNumber).ToList()
            };

            Conversation.Add(message);
            App.brockerConnection.SendMessage(message);

            messageText.Text = "";
        }

        public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			TableView.AllowsSelection = (Conversation == null);
			sendButton.Enabled = (Conversation != null);
			keyboardObserver = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillChangeFrameNotification, OnKeyboardChangeFrame);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardObserver, UIKeyboard.WillChangeFrameNotification, null);
		}

		void OnKeyboardChangeFrame (NSNotification obj)
		{
			var frame = View.ConvertRectFromView (UIKeyboard.FrameEndFromNotification (obj), null);
			var curve = UIKeyboard.AnimationCurveFromNotification (obj);
			var offset = View.Bounds.Height - frame.Y;
			View.LayoutIfNeeded ();
			UIView.Animate (
				duration: UIKeyboard.AnimationDurationFromNotification (obj),
				delay: 0,
				options: (UIViewAnimationOptions)(curve << 16),
				animation: () => {
					messageEntryBottom.Constant = offset - BottomLayoutGuide.Length;
					View.LayoutIfNeeded ();
					var ci = TableView.ContentInset;
					ci.Bottom = offset + messageEntry.Frame.Height;
					TableView.ContentInset = ci;
				},
				completion: null
			);
		}

		protected virtual UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath, object data)
		{
			var msg = data as Message;
			if (msg != null) {
                var cellId = msg.Sender.PhoneNumber == App.contactManager.Me.PhoneNumber ? MessageCell.OutgoingId : MessageCell.IncomingId;
				var cell = (MessageCell)tableView.DequeueReusableCell (cellId, indexPath);
				cell.Message = msg;
				return cell;
			}

			var contact = data as Contact;
			if (contact != null) {
				var cell = tableView.DequeueReusableCell (contactCellId, indexPath);
				cell.TextLabel.Text = contact.FirstName + " " + contact.LastName;
				return cell;
			}
			throw new NotImplementedException (data.GetType ().FullName);
		}

		void SetDataBinding ()
		{
			var ds = DataSource;
			if (ds != null)
                ds.Binding = ((IList)Conversation?.Messages) ?? ((IList)App.contactManager.GetContacts ());
		}

		void SetTitleBinding ()
		{
			// If someone joins the conversation, we'll update the title..
			titleBinding?.Dispose ();
			titleBinding = Conversation?.ObservePropertyValue (c => c.Participants)
				.ObserveOn (SynchronizationContext.Current)
				.Subscribe (participants =>
                            Title = string.Join (", ", participants.Where (p => p.PhoneNumber != App.contactManager.Me.PhoneNumber))
				);
		}

		[Export ("tableView:didSelectRowAtIndexPath:")]
		public async void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			// Get the contact that was selected
			var contact = (Contact)DataSource.GetData (indexPath);

			// Clear the table while we load
			DataSource.Binding = null;

			// Create or resume the conversation
			Conversation = await App.conversationManager.StartConversation (contact);
			SetDataBinding ();

			// Enable send button
			sendButton.Enabled = true;
			TableView.AllowsSelection = false;
		}
	}
}