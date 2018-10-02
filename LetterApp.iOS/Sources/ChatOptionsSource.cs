using System;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class ChatOptionsSource : UITableViewSource
    {
        private readonly bool _muted;
        private readonly string[] _resources;

        public event EventHandler<ChatOptions> OptionSelectedEvent;
        public event EventHandler<bool> OptionMuteEvent;

        public ChatOptionsSource(UITableView tableView, bool muted, string[] resources)
        {
            _muted = muted;
            _resources = resources;
            tableView.RegisterNibForCellReuse(LabelWithArrowCell.Nib, LabelWithArrowCell.Key);
            tableView.RegisterNibForCellReuse(SwitchCell.Nib, SwitchCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (indexPath.Row)
            {
                case (int)ChatOptions.SeeProfile:
                    var profileCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                    profileCell.ConfigureForChat(_resources[(int)ChatOptions.SeeProfile], hasArrow: true);
                    cell = profileCell;
                    break;

                case (int)ChatOptions.SendEmail:
                    var emailCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                    emailCell.ConfigureForChat(_resources[(int)ChatOptions.SendEmail]);
                    cell = emailCell;
                    break;

                case (int)ChatOptions.MuteChat:
                    var muteCell = tableView.DequeueReusableCell(SwitchCell.Key) as SwitchCell;
                    muteCell.ConfigureForChat(_resources[(int)ChatOptions.MuteChat], _muted, OptionMuteEvent);
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    cell = muteCell;
                    break;

                case (int)ChatOptions.ArchiveChat:
                    var archiveCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                    archiveCell.ConfigureForChat(_resources[(int)ChatOptions.ArchiveChat], UITextAlignment.Center);
                    cell = archiveCell;
                    break;
            }
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            if (indexPath.Row == (int)ChatOptions.MuteChat)
                return;

            OptionSelectedEvent?.Invoke(this, (ChatOptions)indexPath.Row);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if(indexPath.Row == (int)ChatOptions.ArchiveChat)
                return LocalConstants.Settings_GenericCells + 20;

            return LocalConstants.Settings_GenericCells;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => (int)ChatOptions.Count;
    }
}
