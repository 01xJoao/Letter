using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class SelectDivisionsSource : UITableViewSource
    {
        public SelectDivisionsSource(UITableView tableView, UIView backgroundView, List<DivisionModel> divisions)
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
