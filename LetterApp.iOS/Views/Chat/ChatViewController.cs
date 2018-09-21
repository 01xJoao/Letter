using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Chat
{
    public partial class ChatViewController : XViewController<ChatViewModel>, IUITextViewDelegate
    {
        private int _lineCount;
        private bool _keyboardState;
        private nfloat _keyboardHeight;
        private UIPanGestureRecognizer tableViewGesture = new UIPanGestureRecognizer();

        public override bool HandlesKeyboardNotifications => true;

        public ChatViewController() : base("ChatViewController", null) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _textView.Delegate = this;
            tableViewGesture.AddTarget(() => HandleTableDragGesture(tableViewGesture));

            ConfigureView();
            ConfigureTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    break;
            }
        }

        private void ConfigureView()
        {
            //_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            _imageView1.Image?.Dispose();
            _imageView2.Image?.Dispose();
            _imageView3.Image?.Dispose();

            _imageView1.Image = UIImage.FromBundle("keyboard");
            _imageView2.Image = UIImage.FromBundle("files");
            _imageView3.Image = UIImage.FromBundle("photo_picker");

            _keyboardAreaView.BackgroundColor = Colors.KeyboardView;

            _textView.TextContainerInset = new UIEdgeInsets(10, 10, 12, 10);

            _textView.Text = "Type something...";
            _textView.TextColor = Colors.ProfileGrayDarker;
            _textViewHeightConstraint.Constant = 45;
            _keyBoardAreaViewHeightConstraint.Constant = 89;
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

        private void OnSendButton_TouchUpInside(object sender, EventArgs e)
        {
        }

        public override void OnKeyboardNotification(UIKeyboardEventArgs keybordEvent, bool keyboardState)
        {
            if (keyboardState != _keyboardState)
            {
                _keyboardState = keyboardState;

                if (keyboardState)
                {
                    _tableView.AddGestureRecognizer(tableViewGesture);
                    _keyboardHeight = keybordEvent.FrameEnd.Height;
                }

                UIViewAnimationExtensions.AnimateView(_keyboardAreaView, keybordEvent.FrameEnd.Height, keyboardState);

                OnKeyboardChanged(keyboardState, _keyboardHeight);
            }
        }

        [Export("textViewShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextView textView)
        {
            _imageView1.Image = UIImage.FromBundle("keyboard_active");

            if (!_textView.TranslatesAutoresizingMaskIntoConstraints)
            {
                _textView.TranslatesAutoresizingMaskIntoConstraints = true;
                _keyboardAreaView.TranslatesAutoresizingMaskIntoConstraints = true;

                _textView.SetNeedsLayout();
                _textView.LayoutIfNeeded();

                _keyboardAreaView.SetNeedsLayout();
                _keyboardAreaView.LayoutIfNeeded();
            }

            return true;
        }

        [Export("textViewDidChange:")]
        public void Changed(UITextView textView)
        {
            if(!string.IsNullOrEmpty(textView.Text) && _sendView.BackgroundColor != Colors.SenderButton)
            {
                _sendView.BackgroundColor = Colors.SenderButton;
                _sendButton.SetTitleColor(Colors.White, UIControlState.Normal);
                _sendButton.Enabled = true;
            }
            else if(string.IsNullOrEmpty(textView.Text))
            {
                _sendView.BackgroundColor = UIColor.Clear;
                _sendButton.SetTitleColor(Colors.ProfileGray, UIControlState.Normal);
                _sendButton.Enabled = false;
            }

            int lineCount = (int)(textView.ContentSize.Height / textView.Font.LineHeight) - 2;

            if(lineCount < 5 && lineCount != _lineCount)
            {
                _textView.Frame = new CGRect(_textView.Frame.X, _textView.Frame.Y, _textView.Frame.Width,
                                             _textViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                int keyHeight = _keyboardState ? (int)_keyboardHeight : 0;
                var keyViewY = this.View.Frame.Height - (keyHeight + _keyBoardAreaViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                _keyboardAreaView.Frame = new CGRect(_keyboardAreaView.Frame.X, keyViewY, _keyboardAreaView.Frame.Width, 
                                                     _keyBoardAreaViewHeightConstraint.Constant + (lineCount * (int)textView.Font.LineHeight));

                if (!_textView.TranslatesAutoresizingMaskIntoConstraints)
                {
                    _textView.TranslatesAutoresizingMaskIntoConstraints = true;
                    _keyboardAreaView.TranslatesAutoresizingMaskIntoConstraints = true;
                }

                _textView.SetNeedsLayout();
                _textView.LayoutIfNeeded();

                _keyboardAreaView.SetNeedsLayout();
                _keyboardAreaView.LayoutIfNeeded();

                OnKeyboardChanged(_keyboardState, _keyboardHeight);

                _lineCount = lineCount;
            }
        }

        [Export("textViewDidEndEditing:")]
        public void EditingEnded(UITextView textView)
        {
            _imageView1.Image = UIImage.FromBundle("keyboard");
        }

        private void OnButton1_TouchUpInside(object sender, EventArgs e)
        {
            if(!_keyboardState)
                _textView.BecomeFirstResponder();
        }

        private void OnButton2_TouchUpInside(object sender, EventArgs e)
        {
        }

        private void OnButton3_TouchUpInside(object sender, EventArgs e)
        {
        }

        private void ConfigureTableView()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var titleViewMaxSize = UIScreen.MainScreen.Bounds.Width - 165; //165 is the size and icons space

            this.NavigationItem.TitleView = CustomUIExtensions.SetupNavigationBarWithSubtitle("João Palma", "ESTG - Aluno ECGM", titleViewMaxSize);
            this.NavigationController.NavigationBar.TintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes { ForegroundColor = Colors.White };
            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(44, "back_white", CloseView);

            this.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] {
                UIButtonExtensions.SetupBarWithTwoButtons(44, "options", Options),
                UIButtonExtensions.SetupBarWithTwoButtons(44, "call_white_medium", CallUser)
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

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

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

        private void HandleTableDragGesture(UIPanGestureRecognizer tableViewGesture)
        {
            _textView.ResignFirstResponder();
            _tableView.RemoveGestureRecognizer(tableViewGesture);
        }


        #region ScrollView

        private void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            UIScrollView scrollView = _tableView as UIScrollView;

            if (visible)
                CenterViewInScroll(_tableView, scrollView, keyboardHeight);
            else
                RestoreScrollPosition(scrollView);
        }

        private void CenterViewInScroll(UIView viewToCenter, UIScrollView scrollView, nfloat keyboardHeight)
        {
            var contentInsets = new UIEdgeInsets(0, 0, keyboardHeight, 0);
            scrollView.ContentInset = contentInsets;
            scrollView.ScrollIndicatorInsets = contentInsets;

            // Position of the active field relative inside the scroll view
            var relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

            var landscape = TraitCollection.VerticalSizeClass == UIUserInterfaceSizeClass.Compact;
            var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

            if (relativeFrame.Y + relativeFrame.Height + 16 > spaceAboveKeyboard)
            {
                // Move the active field to the center of the available space
                var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
                scrollView.ContentOffset = new CGPoint(0, (float)offset);
            }
        }

        private void RestoreScrollPosition(UIScrollView scrollView)
        {
            scrollView.ContentInset = UIEdgeInsets.Zero;
            scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        #endregion
    }
}

