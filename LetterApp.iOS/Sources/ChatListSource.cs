using System;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class ChatListSource : UITableViewSource
    {
        public ChatListSource(UITableView tableView, object chats)
        {
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return new UITableViewCell();
        }

        public override nint RowsInSection(UITableView tableview, nint section) => 0;
    }
}
