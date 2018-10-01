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

namespace LetterApp.iOS.Views.Member
{
    public partial class MemberViewController : XViewController<MemberViewModel>
    {
        public MemberViewController() : base("MemberViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.MemberProfileModel):
                    UpdateView();
                    break;
                default:
                    break;
            }
        }

        private void SetupView()
        {
            _backHeightConstraint.Constant = _backHeightConstraint.Constant + (PhoneModelExtensions.IsIphoneX() ? 20 : 0); 

            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            _headerView.BackgroundColor = Colors.MainBlue;
            _backImage.Image = UIImage.FromBundle("back_white");

            _chatView.BackgroundColor = Colors.ConnectViewButton1;
            _callView.BackgroundColor = Colors.ConnectViewButton2;

            UIButtonExtensions.SetupButtonAppearance(_chatButton, Colors.MainBlue, 15f, ViewModel.ChatLabel);
            UIButtonExtensions.SetupButtonAppearance(_callButton, Colors.MainBlue, 15f, ViewModel.CallLabel);

            _backButton.TouchUpInside -= OnBackButton_TouchUpInside;
            _backButton.TouchUpInside += OnBackButton_TouchUpInside;
        }

        private void UpdateView()
        {
            _profileImage.Image?.Dispose();
            ImageService.Instance.LoadStream((token) => {
                return ImageHelper.GetStreamFromImageByte(token, ViewModel.MemberProfileModel.Picture);
            }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_profileImage);
            CustomUIExtensions.RoundShadow(_profileImage);

            UILabelExtensions.SetupLabelAppearance(_nameLabel, $"{ViewModel.MemberProfileModel.FirstName} {ViewModel.MemberProfileModel.LastName}", Colors.White, 22f);

            if (!string.IsNullOrEmpty(ViewModel.MemberProfileModel.Description))
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.MemberProfileModel.Description, Colors.White, 13f);
            else
                UILabelExtensions.SetupLabelAppearance(_descriptionLabel, ViewModel.MemberNoDescriptionLabel, Colors.ProfileGrayWhiter, 13f, italic: true);

            _tableView.Source = new MemberSource(_tableView, ViewModel.MemberDetails);
            _tableView.ReloadData();

            _chatButton.TouchUpInside -= OnChatButton_TouchUpInside;
            _chatButton.TouchUpInside += OnChatButton_TouchUpInside;

            _callButton.TouchUpInside -= OnCallButton_TouchUpInside;
            _callButton.TouchUpInside += OnCallButton_TouchUpInside;   
        }

        private void OnCallButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.CallCommand.Execute();
        }

        private void OnChatButton_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.ChatCommand.Execute();
        }

        private void OnBackButton_TouchUpInside(object sender, EventArgs e)
        {
            if (ViewModel.CloseViewCommand.CanExecute())
                ViewModel.CloseViewCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.SetNavigationBarHidden(false, false);
            this.NavigationController.NavigationBar.Hidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            this.NavigationController.SetNavigationBarHidden(true, false);
            this.NavigationController.NavigationBar.Hidden = false;
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

