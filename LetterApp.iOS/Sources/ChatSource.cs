using System;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class ChatSource : UITableViewSource
    {
        public ChatSource(UITableView tableView)
        {
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            throw new NotImplementedException();
        }
    }
}
