﻿using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.TabBar.ChatListViewController.Cells;
using UIKit;
using static LetterApp.Core.ViewModels.TabBarViewModels.ChatListViewModel;

namespace LetterApp.iOS.Sources
{
    public class ChatListSource : UITableViewSource
    {
        private readonly string[] _textResources;
        private readonly List<ChatListUserCellModel> _chats;

        public event EventHandler<Tuple<ChatEventType, int>> ChatListActionsEvent;

        public ChatListSource(UITableView tableView, List<ChatListUserCellModel> chats, string[] textResources)
        {
            _chats = chats;
            _textResources = textResources;
            tableView.RegisterNibForCellReuse(ChatCell.Nib, ChatCell.Key);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(ChatCell.Key) as ChatCell;
            cell.Configure(_chats[indexPath.Row]);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction archiveButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Destructive,
                _textResources[0],
                delegate {
                    tableView.BeginUpdates();
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Bottom);
                    ChatListActionsEvent?.Invoke(this, new Tuple<ChatEventType, int> (ChatEventType.Archive, _chats[indexPath.Row].MemberId));
                    _chats.RemoveAt(indexPath.Row);
                    tableView.EndUpdates();
                    tableView.ReloadData();
                });

            archiveButton.BackgroundColor = Colors.GrayIndicator;

            UITableViewRowAction infoButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Normal,
                _chats[indexPath.Row].IsMemberMuted ? _textResources[2] : _textResources[1],
                delegate {
                    tableView.BeginUpdates();
                    ChatListActionsEvent?.Invoke(this, new Tuple<ChatEventType, int>(ChatEventType.Mute, _chats[indexPath.Row].MemberId));
                    tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Right);
                    tableView.EndUpdates();
                });

            infoButton.BackgroundColor = Colors.Orange;

            return new UITableViewRowAction[] { archiveButton, infoButton };
        }

        public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.SetEditing(false, true);
            return null;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => LocalConstants.Chats_ChatCellHeight;

        public override nint RowsInSection(UITableView tableview, nint section) => _chats.Count;
    }
}
