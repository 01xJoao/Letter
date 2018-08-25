using System;
using System.ComponentModel;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController
{
    public partial class CallListViewController : XViewController<CallListViewModel>
    {
        public CallListViewController() : base("CallListViewController", null) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetupTableView();
        }

        private void ConfigureView()
        {
            this.Title = ViewModel.Title;
            this.NavigationController.NavigationBar.PrefersLargeTitles = true;
            this.NavigationItem.RightBarButtonItem = UIButtonExtensions.SetupImageBarButton(20f, "new_call", OpenContacts);
            _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        private void SetupTableView()
        {
            var source = new CallSource(_tableView, ViewModel.CallHistory);
            _tableView.Source = source;

            source.CallEvent -= OnSource_CallEvent;
            source.CallEvent += OnSource_CallEvent;

            source.OpenCallerProfileEvent -= OnSource_OpenCallerProfileEvent;
            source.OpenCallerProfileEvent += OnSource_OpenCallerProfileEvent;

            _tableView.ReloadData();
        }

        private void OnSource_OpenCallerProfileEvent(object sender, int callerId)
        {
            //Viewmodel.OpenCallerProfile.Execute(callerId)
        }

        private void OnSource_CallEvent(object sender, int callerId)
        {
            //ViewModel.CallCommand.Execute(callerId)
        }

        private void OpenContacts(object sender, EventArgs e)
        {
            //ViewModel.OpenCallListCommand.CanExecute()
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.HidesBottomBarWhenPushed = false;
        }
    }
}

