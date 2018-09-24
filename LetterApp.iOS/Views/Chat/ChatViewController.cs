using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CoreGraphics;
using Foundation;
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

        private UILabel _statusLabel;
        private int _lineCount;
        private bool _keyboardState;
        private UIPanGestureRecognizer tableViewGesture = new UIPanGestureRecognizer();
        private UIScrollView _tableScrollView;
        private int _keyboardHeight;
        private bool _scrollToBottom;

        public ChatViewController() : base("ChatViewController", null) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _sendButton.Enabled = false;
            _tableView.ContentInset = new UIEdgeInsets(-36, 0, -36, 0);
            _textView.Delegate = this;
            tableViewGesture.AddTarget(() => HandleTableDragGesture(tableViewGesture));

            ConfigureView();

            Debug.WriteLine("DidLoadView:" + ViewModel.Chat?.Messages?.Count);
            if (ViewModel.Chat?.Messages?.Count > 0)
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
                    break;
                case nameof(ViewModel.Status):
                    break;
            }
        }

        private void ConfigureView()
        {
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.BackgroundColor = Colors.White;

            //TODO Update status Label
            _statusLabel = new UILabel(new CGRect(0, _tableView.Frame.Height - 20, ScreenWidth, 20)) { TextAlignment = UITextAlignment.Center };
            _tableView.TableFooterView = new UIView();
            _tableView.TableFooterView.AddSubview(_statusLabel);

            _imageView1.Image?.Dispose();
            _imageView2.Image?.Dispose();
            _imageView3.Image?.Dispose();

            _imageView1.Image = UIImage.FromBundle("keyboard");
            _imageView2.Image = UIImage.FromBundle("files");
            _imageView3.Image = UIImage.FromBundle("photo_picker");

            _keyboardAreaView.BackgroundColor = Colors.KeyboardView;

            _textView.TextContainerInset = new UIEdgeInsets(10, 10, 12, 10);
            _textView.Text = ViewModel.TypeSomething;
            _textView.TextColor = Colors.ProfileGrayDarker;
            _textView.Font = UIFont.SystemFontOfSize(14f);

            CustomUIExtensions.CornerView(_sendView, 2);
            UIButtonExtensions.SetupButtonAppearance(_sendButton, Colors.ProfileGray, 15f, "Send", UIFontWeight.Medium);

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
            Debug.WriteLine("ENTER HERE!!!!!!!!!! : ");

            if (ViewModel.Chat.Messages != null && ViewModel.Chat.Messages.Count > 0)
            {
                _tableView.Source = new ChatSource(_tableView, ViewModel.Chat);
                _tableView.ReloadData();

                if (_scrollToBottom)
                {
                    _scrollToBottom = false;
                    ScrollToBottom();
                }
            }
            else
            {
                _scrollToBottom = true;
            }
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool keyboardState)
        {
            if (keyboardState != _keyboardState)
            {
                _keyboardState = keyboardState;

                if (keyboardState)
                    _tableView.AddGestureRecognizer(tableViewGesture);

                _keyboardHeight = (int)keybordEvent.FrameEnd.Height;

                _keyboardViewBottomConstraint.Constant = keyboardState ? _keyboardHeight : 0;
                _tableViewBottomConstraint.Constant = keyboardState ? _keyboardHeight : _keyboardAreaView.Frame.Height;

                UIView.Animate(0.3f, this.View.LayoutIfNeeded);

                AnimateTableView(keyboardState);
            }
        }

        private void AnimateTableView(bool keyboardState)
        {
            if (_tableScrollView == null)
                _tableScrollView = _tableView as UIScrollView;

            if (keyboardState)
            {
                if (_tableScrollView.ContentSize.Height < _tableScrollView.Bounds.Size.Height)
                    return;

                CGPoint bottomOffset = new CGPoint(0, _tableScrollView.ContentSize.Height - (_tableScrollView.Bounds.Size.Height - _keyboardAreaView.Frame.Height) - 36);
                _tableScrollView.SetContentOffset(bottomOffset, true);
            }
            else
            {
                _tableScrollView.ContentInset = UIEdgeInsets.Zero;
                _tableScrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
            }

            _tableView.ContentInset = new UIEdgeInsets(-36, 0, -36, 0);
        }

        private void HandleTableDragGesture(UIPanGestureRecognizer tableViewGesture)
        {
            _textView.ResignFirstResponder();
            _tableView.RemoveGestureRecognizer(tableViewGesture);
        }

        [Export("textViewShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextView textView)
        {
            if (textView.Text == ViewModel.TypeSomething)
                textView.Text = "";

            _imageView1.Image = UIImage.FromBundle("keyboard_active");
            return true;
        }

        [Export("textViewDidChange:")]
        public void Changed(UITextView textView)
        {
            if (!string.IsNullOrEmpty(textView.Text) && _sendView.BackgroundColor != Colors.SenderButton)
            {
                _sendView.BackgroundColor = Colors.SenderButton;
                _sendButton.SetTitleColor(Colors.White, UIControlState.Normal);
                _sendButton.Enabled = true;
            }
            else if (string.IsNullOrEmpty(textView.Text) && _keyboardState == false)
            {
                textView.Text = ViewModel.TypeSomething;
                _sendView.BackgroundColor = UIColor.Clear;
                _sendButton.SetTitleColor(Colors.ProfileGray, UIControlState.Normal);
                _sendButton.Enabled = false;
            }

            int lineCount = (int)(textView.ContentSize.Height / textView.Font.LineHeight) - 2;

            if (lineCount < 5 && lineCount != _lineCount)
            {
                //_textView.Frame = new CGRect(_textView.Frame.X, _textView.Frame.Y, _textView.Frame.Width,
                //_textViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                //int keyHeight = _keyboardState ? (int)_keyboardHeight : 0;
                //var keyViewY = this.View.Frame.Height - (keyHeight + _keyBoardAreaViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                //_keyboardAreaView.Frame = new CGRect(_keyboardAreaView.Frame.X, keyViewY, _keyboardAreaView.Frame.Width, 
                //                                     _keyBoardAreaViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                //if (!_textView.TranslatesAutoresizingMaskIntoConstraints)
                //{
                //    _textView.TranslatesAutoresizingMaskIntoConstraints = true;
                //    _keyboardAreaView.TranslatesAutoresizingMaskIntoConstraints = true;
                //}

                ////_textView.SetNeedsLayout();
                ////_textView.LayoutIfNeeded();

                //_keyboardAreaView.SetNeedsLayout();
                //_keyboardAreaView.LayoutIfNeeded();

                ////OnKeyboardChanged(_keyboardState, _keyboardHeight + _keyboardAreaView.Frame.Height);

                //_lineCount = lineCount;
            }
        }

        [Export("textViewDidEndEditing:")]
        public void EditingEnded(UITextView textView)
        {
            if (string.IsNullOrEmpty(textView.Text))
                textView.Text = ViewModel.TypeSomething;

            _imageView1.Image = UIImage.FromBundle("keyboard");
        }

        private void OnButton1_TouchUpInside(object sender, EventArgs e)
        {
            if (!_keyboardState)
                _textView.BecomeFirstResponder();
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
                ViewModel.SendMessageCommand.Execute(_textView.Text);
                _textView.Text = string.Empty;
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

       private void ScrollToBottom()
       {
            if (_tableView.ContentSize.Height > _tableView.Bounds.Size.Height)
            {
                var offSet = _tableView.ContentSize.Height - _tableView.Bounds.Size.Height;
                _tableView.SetContentOffset(new CGPoint(0, offSet), false);
            }
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

            _tableView.EstimatedRowHeight = 30;
            _tableView.RowHeight = UITableView.AutomaticDimension;
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
                tableViewGesture = null;
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

