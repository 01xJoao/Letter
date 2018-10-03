﻿using System;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Chat.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class ChatSource : UITableViewSource
    {
        private readonly nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
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
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            int messageIndex = indexPath.Row;

            var cell = new UITableViewCell();

            if (_chat.Messages.Count == messageIndex)
                return cell;

            switch (_chat.Messages[messageIndex].PresentMessage)
            {
                case PresentMessageType.UserText:
                    var userTextCell = tableView.DequeueReusableCell(MessageCell.Key) as MessageCell;
                    userTextCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent, _chat.MemberPresence);
                    cell = userTextCell;
                    break;
                case PresentMessageType.UserImage:
                    var userImageCell = tableView.DequeueReusableCell(ImageWithUserCell.Key) as ImageWithUserCell;
                    userImageCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent, _chat.MemberPresence);
                    cell = userImageCell;
                    break;
                case PresentMessageType.UserFile:
                    var userFileCell = tableView.DequeueReusableCell(FileWithUserCell.Key) as FileWithUserCell;
                    userFileCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent, _chat.MemberPresence);
                    cell = userFileCell;
                    break;
                case PresentMessageType.Text:
                    var textCell = tableView.DequeueReusableCell(LabelCell.Key) as LabelCell;
                    textCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent);
                    cell = textCell;
                    break;
                case PresentMessageType.Image:
                    var imageCell = tableView.DequeueReusableCell(ImageCell.Key) as ImageCell;
                    imageCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent);
                    cell = imageCell;
                    break;
                case PresentMessageType.File:
                    var fileCell = tableView.DequeueReusableCell(FileCell.Key) as FileCell;
                    fileCell.Configure(_chat.Messages[messageIndex], _chat.MessageEvent);
                    cell = fileCell;
                    break;
            }

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => _chat.Messages.Count;

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            float cellHeight = 2;
            int messageIndex = indexPath.Row;

            var message = _chat.Messages[messageIndex];
            var approximateWidthOfText = _screenWidth - 75;
            var size = new CoreGraphics.CGSize(approximateWidthOfText, 1000);

            var paragraphStyle = new NSMutableParagraphStyle
            {
                LineSpacing = 2f,
                MinimumLineHeight = UIFont.SystemFontOfSize(14).LineHeight,
                MaximumLineHeight = UIFont.SystemFontOfSize(14).LineHeight
            };

            var attributes = new UIStringAttributes { Font = UIFont.SystemFontOfSize(14), ParagraphStyle = paragraphStyle };
            var estimatedFrame = new NSString(message.MessageData).GetBoundingRect(size, NSStringDrawingOptions.UsesLineFragmentOrigin, attributes, null);

            switch (message.PresentMessage)
            {
                case PresentMessageType.UserText:
                    cellHeight += 18.5f + (_chat.Messages[indexPath.Row].ShowHeaderDate ? LocalConstants.Chat_HeaderDateBig : LocalConstants.Chat_HeaderDateSmall);
                    break;
            }

            return estimatedFrame.Height + cellHeight;
        }
    }
}
