using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Chat.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class ChatSource : UITableViewSource
    {
        private readonly ChatModel _chat;

        public ChatSource(UITableView tableView, ChatModel chat)
        {
            _chat = chat;

            tableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
            tableView.RegisterNibForCellReuse(LabelCell.Nib, LabelCell.Key);
            tableView.RegisterNibForCellReuse(FileCell.Nib, FileCell.Key);
            tableView.RegisterNibForCellReuse(FileWithUserCell.Nib, FileWithUserCell.Key);
            tableView.RegisterNibForCellReuse(ImageCell.Nib, ImageCell.Key);
            tableView.RegisterNibForCellReuse(ImageWithUserCell.Nib, ImageWithUserCell.Key);
            tableView.RegisterNibForCellReuse(DateHeaderCell.Nib, DateHeaderCell.Key);
        }
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var view = tableView.DequeueReusableCell(DateHeaderCell.Key) as DateHeaderCell;
            view.Configure(_chat.SectionsAndRowsCount[(int)section].Item1);
            return view;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => LocalConstants.Chat_HeaderDate;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (_chat.Messages[indexPath.Row].PresentMessage)
            {
                case PresentMessageType.UserText:
                    var userTextCell = tableView.DequeueReusableCell(MessageCell.Key) as MessageCell;
                    userTextCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent, _chat.MemberName, _chat.MemberPhoto, _chat.MemberPresence);
                    cell = userTextCell;
                    break;
                case PresentMessageType.UserImage:
                    var userImageCell = tableView.DequeueReusableCell(ImageWithUserCell.Key) as ImageWithUserCell;
                    userImageCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent, _chat.MemberName, _chat.MemberPhoto, _chat.MemberPresence);
                    cell = userImageCell;
                    break;
                case PresentMessageType.UserFile:
                    var userFileCell = tableView.DequeueReusableCell(FileWithUserCell.Key) as FileWithUserCell;
                    userFileCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent, _chat.MemberName, _chat.MemberPhoto, _chat.MemberPresence);
                    cell = userFileCell;
                    break;
                case PresentMessageType.Text:
                    var textCell = tableView.DequeueReusableCell(LabelCell.Key) as LabelCell;
                    textCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent);
                    cell = textCell;
                    break;
                case PresentMessageType.Image:
                    var imageCell = tableView.DequeueReusableCell(ImageCell.Key) as ImageCell;
                    imageCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent);
                    cell = imageCell;
                    break;
                case PresentMessageType.File:
                    var fileCell = tableView.DequeueReusableCell(FileCell.Key) as FileCell;
                    fileCell.Configure(_chat.Messages[indexPath.Row], _chat.MessageEvent);
                    cell = fileCell;
                    break;
            }

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => _chat.SectionsAndRowsCount[(int)section].Item2;
        public override nint NumberOfSections(UITableView tableView) => _chat.SectionsAndRowsCount.Count;
    }
}
