using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;
using static LetterApp.Core.ViewModels.TabBarViewModels.ChatListViewModel;

namespace LetterApp.iOS.Sources
{
    public class ChatListSource : UITableViewSource
    {
        private string[] _textResources;
        private List<ChatListUserCellModel> _chats;

        public event EventHandler<Tuple<ChatEventType, int>> ChatEvent;

        public ChatListSource(UITableView tableView, List<ChatListUserCellModel> chats, string[] textResources)
        {
            _chats = chats;
            _textResources = textResources;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return new UITableViewCell();
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction deleteButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Destructive,
                _textResources[0],
                delegate {
                    tableView.BeginUpdates();
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Bottom);
                    ChatEvent?.Invoke(this, new Tuple<ChatEventType, int> (ChatEventType.Delete, indexPath.Row));
                    tableView.EndUpdates();
                    tableView.ReloadData();
                });

            deleteButton.BackgroundColor = Colors.Red;

            UITableViewRowAction infoButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Normal,
                //TODO Add logic if user is muted or not
                _chats[indexPath.Row].IsMemberMuted ? _textResources[1] : _textResources[2],
                delegate {
                    ChatEvent?.Invoke(this, new Tuple<ChatEventType, int>(ChatEventType.Mute, indexPath.Row));
                });

            infoButton.BackgroundColor = Colors.Orange;

            return new UITableViewRowAction[] { deleteButton, infoButton };
        }

        public override nint RowsInSection(UITableView tableview, nint section) => 0;
    }
}
