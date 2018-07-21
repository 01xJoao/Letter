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

        private void SetupView()
        {
            this.Title = ViewModel.Division.Name;

            _profileHeaderView.BackgroundColor = Colors.MainBlue;
            _profileImage.Image?.Dispose();

            UILabelExtensions.SetupLabelAppearance(_membersLabel, $"{ViewModel.Division.UserCount} {ViewModel.MembersLabel}", Colors.White, 13f);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.Division.Description, Colors.White, 13f);

            _memberImage.Image = UIImage.FromBundle("members");

            _profileImage.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, ViewModel.Division.Picture);
            }).LoadingPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_profileImage);
            CustomUIExtensions.RoundShadow(_profileImage);

            _buttonView1.BackgroundColor = Colors.ConnectViewButton1;
            _buttonView2.BackgroundColor = Colors.ConnectViewButton2;

            UIButtonExtensions.SetupButtonAppearance(_button1, Colors.MainBlue, 15f, ViewModel.SendEmailLabel);
            UIButtonExtensions.SetupButtonAppearance(_button2, Colors.MainBlue, 15f, ViewModel.CallLabel);

            _tableView.Source = new DivisionSource(_tableView, ViewModel.OrganizationInfo, ViewModel.ProfileDetails);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = Colors.White };
            var backButton = UIButtonExtensions.SetupImageBarButton(20, "back_white", CloseView);
            this.NavigationItem.LeftBarButtonItem = backButton;
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
            if (this.IsMovingFromParentViewController)
            {
                base.ViewDidDisappear(animated);
                MemoryUtility.ReleaseUIViewWithChildren(this.View);
            }
        }
    }
}

