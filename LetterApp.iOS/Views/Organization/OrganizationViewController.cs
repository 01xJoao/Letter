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

namespace LetterApp.iOS.Views.Organization
{
    public partial class OrganizationViewController : XViewController<OrganizationViewModel>
    {
        public OrganizationViewController() : base("OrganizationViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Loading(true);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Organization):
                    SetupView();
                    break;
                default:
                    break;
            }
        }

        private void Loading(bool showLoading)
        {
            UIViewAnimationExtensions.CustomViewLoadingAnimation("loading", this.View, _imageView, showLoading);
        }

        private void SetupView()
        {
            Loading(false);

            this.View.BackgroundColor = Colors.MainBlue;
            _tableView.BackgroundColor = Colors.MainBlue4;

            this.Title = ViewModel.Organization.Name;

            if (!string.IsNullOrEmpty(ViewModel.Organization.Description))
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.Organization.Description, Colors.White, 14f);
            else
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.OrganizationNoDescription, Colors.ProfileGrayWhiter, 14f, italic: true);

            _imageView.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, ViewModel.Organization.Picture);
            }).LoadingPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_imageView);

            _tableView.Source = new OrganizationSource(_tableView, ViewModel.ProfileOrganization, ViewModel.ProfileDetails);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _tableView.ReloadData();

            _buttonView1.BackgroundColor = Colors.ConnectViewButton1;
            _buttonView2.BackgroundColor = Colors.ConnectViewButton2;

            UIButtonExtensions.SetupButtonAppearance(_button1, Colors.MainBlue, 15f, ViewModel.SendEmailLabel);
            UIButtonExtensions.SetupButtonAppearance(_button2, Colors.MainBlue, 15f, ViewModel.CallLabel);

            _button1.TouchUpInside -= OnButton1_TouchUpInside;
            _button1.TouchUpInside += OnButton1_TouchUpInside;

            _button2.TouchUpInside -= OnButton2_TouchUpInside;
            _button2.TouchUpInside += OnButton2_TouchUpInside;   
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

