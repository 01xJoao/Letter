using System;
using System.ComponentModel;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using LetterApp.Core.ViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.Division
{
    public partial class DivisionViewController : XViewController<DivisionViewModel>
    {
        public DivisionViewController() : base("DivisionViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Loading(true);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Division):
                    SetupView();
                    break;
                default:
                    break;
            }
        }

        private void Loading(bool showLoading)
        {
            UIViewAnimationExtensions.CustomViewLoadingAnimation("loading", _profileHeaderView, _profileImage, showLoading);
        }

        private void SetupView()
        {
            Loading(false);

            this.Title = ViewModel.Division.Name;
            _profileHeaderView.BackgroundColor = Colors.MainBlue;

            UILabelExtensions.SetupLabelAppearance(_membersLabel, $"{ViewModel.Division.UserCount} {ViewModel.MembersLabel}", Colors.White, 13f);

            if (!string.IsNullOrEmpty(ViewModel.Division.Description))
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.Division.Description, Colors.White, 13f);
            else
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.DivisionNoDescription, Colors.ProfileGrayWhiter, 13f, italic: true);

            _memberImage.Image = UIImage.FromBundle("members");

            _profileImage.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, ViewModel.Division.Picture);
            }).ErrorPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_profileImage);
            CustomUIExtensions.RoundShadow(_profileImage);

            _buttonView1.BackgroundColor = Colors.ConnectViewButton1;
            _buttonView2.BackgroundColor = Colors.ConnectViewButton2;

            UIButtonExtensions.SetupButtonAppearance(_button1, Colors.MainBlue, 15f, ViewModel.SendEmailLabel);
            UIButtonExtensions.SetupButtonAppearance(_button2, Colors.MainBlue, 15f, ViewModel.CallLabel);

            _button1.TouchUpInside -= OnButton1_TouchUpInside;
            _button1.TouchUpInside += OnButton1_TouchUpInside;

            _button2.TouchUpInside -= OnButton2_TouchUpInside;
            _button2.TouchUpInside += OnButton2_TouchUpInside;   

            _tableView.Source = new DivisionSource(_tableView, ViewModel.OrganizationInfo, ViewModel.ProfileDetails);
            _tableView.ReloadData();
        }

        private void OnButton1_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.SendEmailCommand.Execute();
        }

        private void OnButton2_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.CallCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.White };
            this.NavigationItem.LeftBarButtonItem = UIButtonExtensions.SetupImageBarButton(20, "back_white", CloseView);
            this.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
            this.NavigationController.NavigationBar.BarTintColor = Colors.MainBlue;
            this.NavigationController.NavigationBar.Translucent = false;
            this.NavigationController.SetNavigationBarHidden(false, true);
            this.NavigationController.NavigationBar.ShadowImage = new UIImage();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
        }

        private void CloseView(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (this.IsMovingFromParentViewController)
            {
                this.NavigationController.SetNavigationBarHidden(true, true);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.IsMovingFromParentViewController)
            {
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

