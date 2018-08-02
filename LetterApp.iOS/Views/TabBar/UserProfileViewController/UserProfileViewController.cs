﻿using System.ComponentModel;
using CoreGraphics;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.iOS.Views.TabBar.UserProfileViewController.Cells;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.UserProfileViewController
{
    public partial class UserProfileViewController : XViewController<UserProfileViewModel>
    {
        public UserProfileViewController() : base("UserProfileViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationController.NavigationBarHidden = true;
            this.View.BackgroundColor = Colors.MainBlue;
            _statusView.BackgroundColor = Colors.MainBlue;

            _tableView.SetContentOffset(new CGPoint(0, 0), false);
            _tableView.BackgroundColor = Colors.White;
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            SetupTableView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.UpdateView):
                    SetupTableView();
                    break;
            }
        }

        private void SetupTableView()
        {
            _tableView.Source = new UserProfileSource(_tableView, ViewModel.ProfileDetails, ViewModel.ProfileDivision);

            _tableView.TableHeaderView = new UIView(new CGRect(0, 0, 0, LocalConstants.Profile_TableHeaderHeight));
            var tableHeader = ProfileHeaderView.Create();
            tableHeader.Configure(ViewModel.ProfileHeader);
            tableHeader.Frame = _tableView.TableHeaderView.Frame;
            _tableView.TableHeaderView.AddSubview(tableHeader);

            _tableView.ReloadData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

