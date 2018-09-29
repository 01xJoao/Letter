using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Chat
{
    public partial class ChatViewController : XViewController<ChatViewModel>, IUITextViewDelegate
    {
        public override bool HandlesKeyboardNotifications => true;
        public override bool DismissKeyboardOnTap => false;

        private UILabel _statusLabel;
        private UITapGestureRecognizer _tableViewTapGesture = new UITapGestureRecognizer { CancelsTouchesInView = false };
        private UIScrollView _tableScrollView;

        private int _lineCount;
        private bool _keyboardState;
        private int _keyboardHeight;

        public ChatViewController() : base("ChatViewController", null) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _tableView.Hidden = true;

            ConfigureView();
            UpdateTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Chat):
                    UpdateTableView();
                    ScrollToLastRow();
                    break;
                case nameof(ViewModel.SendedMessages):
                    AddNewMessageToTable();
                    break;
                case nameof(ViewModel.Status):
                    ShowStatus();
                    break;
            }
        }

        private void ConfigureView()
        {
            _sendButton.Enabled = false;
            _textView.Delegate = this;

            _tableView.SectionHeaderHeight = 0;
            _tableView.TableHeaderView = new UIView(new CGRect(0, 0, 0, 0.1f));

            _tableView.SectionFooterHeight = 5;
            _tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 5f));

            _tableViewTapGesture.AddTarget(HandleTableDragGesture);
            _tableScrollView = _tableView as UIScrollView;

            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.BackgroundColor = Colors.White;

            _imageView1.Image?.Dispose();
            _imageView2.Image?.Dispose();
            _imageView3.Image?.Dispose();

            _imageView1.Image = UIImage.FromBundle("keyboard");
            _imageView2.Image = UIImage.FromBundle("files");
            _imageView3.Image = UIImage.FromBundle("photo_picker");

            _keyboardAreaView.BackgroundColor = Colors.KeyboardView;

            _textView.EnablesReturnKeyAutomatically = true;
            _textView.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
            _textView.TextColor = Colors.Black;
            _textView.Font = UIFont.SystemFontOfSize(14f);
            _textView.Text = string.Empty;

            CustomUIExtensions.CornerView(_sendView, 2);
            UILabelExtensions.SetupLabelAppearance(_sendLabel, ViewModel.SendMessageButton, Colors.ProfileGray, 15f, UIFontWeight.Medium);
            UILabelExtensions.SetupLabelAppearance(_placeholderLabel, ViewModel.TypeSomething, Colors.ProfileGrayDarker, 14f);

            _sendView.BackgroundColor = UIColor.Clear;

            _button1.TouchUpInside -= OnButton1_TouchUpInside;
            _button1.TouchUpInside += OnButton1_TouchUpInside;

            _button2.TouchUpInside -= OnButton2_TouchUpInside;
            _button2.TouchUpInside += OnButton2_TouchUpInside;

            _button3.TouchUpInside -= OnButton3_TouchUpInside;
            _button3.TouchUpInside += OnButton3_TouchUpInside;

            _sendButton.TouchUpInside -= OnSendButton_TouchUpInside;
            _sendButton.TouchUpInside += OnSendButton_TouchUpInside;
        }

        private void UpdateTableView()
        {
            if (ViewModel.Chat?.Messages?.Count > 0)
            {
                _tableView.Source = new ChatSource(_tableView, ViewModel.Chat);
                _tableView.ReloadData();
            }
        }

        private void AddNewMessageToTable()
        {
            UpdateTableView();
            var section = _tableView.NumberOfSections();
            var row = _tableView.NumberOfRowsInSection(section - 1);
            _tableView.ScrollToRow(NSIndexPath.FromRowSection(row - 1, section - 1), UITableViewScrollPosition.Top, false);
        }

        private void ShowStatus(string text = "")
        {
            if (!string.IsNullOrEmpty(text))
            {
                _statusLabel = new UILabel(new CGRect(0, 0, ScreenWidth, 15)) { TextAlignment = UITextAlignment.Center };
                UILabelExtensions.SetupLabelAppearance(_statusLabel, text, Colors.GrayLabel, 11f);

                _tableView.TableFooterView = new UIView();
                _tableView.TableFooterView.AddSubview(_statusLabel);
            }
            else
            {
                _tableView.TableFooterView = new UIView(new CGRect(0, 0, 0, 5f));
                _tableView.SectionFooterHeight = 5;
            }
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool keyboardState)
        {
            if (keyboardState != _keyboardState)
            {
                if (keyboardState)
                {
                    _tableView.AddGestureRecognizer(_tableViewTapGesture);
                    _keyboardHeight = (int)keybordEvent.FrameEnd.Height;
                }
                _keyboardState = keyboardState;
                AnimateTableView(keyboardState);
            }
        }

        private void AnimateTableView(bool showKeyboard)
        {
            _keyboardViewBottomConstraint.Constant = showKeyboard ? _keyboardHeight : 0;
            _tableViewBottomConstraint.Constant = showKeyboard ? _keyboardHeight + _keyboardAreaView.Frame.Height : _keyboardAreaView.Frame.Height;
            UIView.Animate(0.3f, this.View.LayoutIfNeeded);
            ScrollToLastRow();
        }

        private void HandleTableDragGesture()
        {
            _textView.ResignFirstResponder();
            _tableView.RemoveGestureRecognizer(_tableViewTapGesture);
        }

        [Export("textViewShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextView textView)
        {
            _imageView1.Image = UIImage.FromBundle("keyboard_active");
            return true;
        }

        [Export("textViewDidChange:")]
        public void Changed(UITextView textView)
        {
            if (!string.IsNullOrEmpty(textView.Text) && _sendButton.Enabled == false && textView.Text != Environment.NewLine)
            {
                _sendView.BackgroundColor = Colors.SenderButton;
                _sendLabel.TextColor = Colors.White;
                _sendButton.Enabled = true;
                _placeholderLabel.Hidden = true;
            }
            else if (string.IsNullOrEmpty(textView.Text) || textView.Text == Environment.NewLine) {
                DefaultKeyboard();
            }

            int lineCount = (int)(textView.ContentSize.Height / textView.Font.LineHeight) - 2;

            if (lineCount < 4 && lineCount != _lineCount)
            {
                _lineCount = lineCount;
                _keyBoardAreaViewHeightConstraint.Constant = LocalConstants.Chat_KeyboardAreaHeight + (14 * _lineCount);
                _tableViewBottomConstraint.Constant = _keyboardHeight + _keyBoardAreaViewHeightConstraint.Constant;
                UIView.Animate(0.3f, this.View.LayoutIfNeeded);
                textView.ScrollRangeToVisible(new NSRange(0, textView.Text.Length));
            }
        }

        [Export("textViewDidEndEditing:")]
        public void EditingEnded(UITextView textView)
        {
            if (string.IsNullOrEmpty(textView.Text))
                DefaultKeyboard();
        }

        private void DefaultKeyboard()
        {
            _textView.Text = string.Empty;
            _sendView.BackgroundColor = UIColor.Clear;
            _sendLabel.TextColor = Colors.ProfileGray;
            _sendButton.Enabled = false;
            _placeholderLabel.Hidden = false;
            _imageView1.Image = UIImage.FromBundle("keyboard");
        }

        private void OnButton1_TouchUpInside(object sender, EventArgs e)
        {
            if (!_keyboardState)
                _textView.BecomeFirstResponder();
            else
                _textView.ResignFirstResponder();
        }

        private void OnButton2_TouchUpInside(object sender, EventArgs e)
        {
        }

        private void OnButton3_TouchUpInside(object sender, EventArgs e)
        {
        }

        private void OnSendButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.SendMessageCommand.CanExecute(null))
            {
                ViewModel.SendMessageCommand.Execute(new Tuple<string, MessageType>(_textView.Text, MessageType.Text));
                DefaultKeyboard();
            }
        }

        private void Options(object sender, EventArgs e)
        {
        }

        private void CallUser(object sender, EventArgs e)
        {
        }

        private void CloseView(object sender, EventArgs e)
        {
            ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            ScrollToBottom();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ScrollToLastRow();
        }

        private void ScrollToBottom()
        {
            if (_tableView.ContentSize.Height > _tableView.Bounds.Size.Height)
            {
                var offSet = _tableView.ContentSize.Height - _tableView.Bounds.Size.Height;
                _tableView.SetContentOffset(new CGPoint(0, offSet), true);
            }
        }

        private void ScrollToLastRow()
        {
            var section = _tableView.NumberOfSections();
            var row = _tableView.NumberOfRowsInSection(section - 1);

            if (row > 0 && section > 0)
                _tableView.ScrollToRow(NSIndexPath.FromRowSection(row - 1, section - 1), UITableViewScrollPosition.Top, false);

            _tableView.Hidden = false;
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var titleViewMaxSize = ScreenWidth - LocalConstants.Chat_TotalIconsWidth;

            this.NavigationItem.TitleView = CustomUIExtensions.SetupNavigationBarWithSubtitle("João Palma", "ESTG - Aluno", titleViewMaxSize);
            this.NavigationController.NavigationBar.TintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes { ForegroundColor = Colors.White };
            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(LocalConstants.TabBarIconSize, "back_white", CloseView);

            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] {
                UIButtonExtensions.SetupBarWithTwoButtons(LocalConstants.TabBarIconSize, "options", Options),
                UIButtonExtensions.SetupBarWithTwoButtons(LocalConstants.TabBarIconSize, "call_white_medium", CallUser)
            };

            this.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
            this.NavigationController.NavigationBar.BarTintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            this.NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            this.NavigationController.NavigationBar.ShadowImage = new UIImage();
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            _navBarView.BackgroundColor = Colors.MainBlue;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ViewModel.ViewWillCloseCommand.Execute();

            if (this.IsMovingFromParentViewController)
                this.NavigationController.SetNavigationBarHidden(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
            {
                _tableViewTapGesture = null;
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

