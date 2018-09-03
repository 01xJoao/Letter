using System;
using System.Collections.Generic;
using CoreGraphics;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.TabBarViewModels;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController.PageViewController
{
    public partial class ContactPageViewController : XBoardPageViewController
    {
        private string NoResults => L10N.Localize("Contacts_NoResults");
        private UIImageView _noContactsImage = new UIImageView(UIImage.FromBundle("no_contacts"));
        private UILabel _noContactsLabel = new UILabel();
        private bool _showOnlyCalls;
        private int _index;
        private bool _filterByName;
        private EventHandler<Tuple<ContactListViewModel.ContactEventType, int>> _contactEvent;
        private List<GetUsersInDivisionModel> _contactPage;

        public ContactPageViewController(int index, List<GetUsersInDivisionModel> contactPage, EventHandler<Tuple<ContactListViewModel.ContactEventType, int>> contactEvent, bool showOnlyCalls, bool filterByName) 
            : base(index, "ContactPageViewController", null)
        {
            _index = index;
            _contactPage = contactPage;
            _contactEvent = contactEvent;
            _showOnlyCalls = showOnlyCalls;
            _filterByName = filterByName;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupTableView();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _noContactsImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            _noContactsImage.SizeThatFits(_noContactsImage.Image.Size);
            var imageViewCenter = _noContactsImage.Center;
            imageViewCenter.X = _tableView.Bounds.GetMidX();
            imageViewCenter.Y = _tableView.Bounds.GetMidY() - 40;
            _noContactsImage.Center = imageViewCenter;

            _noContactsLabel.Center = this.View.Center;
            var labelY = _noContactsImage.Frame.Y + _noContactsImage.Frame.Height + 40;
            _noContactsLabel.Frame = new CGRect(0, labelY, this.View.Frame.Width, 20);
            UILabelExtensions.SetupLabelAppearance(_noContactsLabel, NoResults, Colors.GrayDivider, 17f, UIFontWeight.Medium);
            _noContactsLabel.TextAlignment = UITextAlignment.Center;

            this.View.AddSubview(_noContactsImage);
            this.View.AddSubview(_noContactsLabel);
        }

        public void SetupTableView(List<GetUsersInDivisionModel> contactPage = null)
        {
            var contact = contactPage ?? _contactPage;

            if (contact.Count > 0)
            {
                _tableView.Hidden = false;
                _noContactsLabel.Hidden = true;
                _noContactsImage.Hidden = true;

                var source = new ContactsSource(_tableView, contact, _showOnlyCalls, _filterByName);

                source.ContactEvent -= OnSource_ContactEvent;
                source.ContactEvent += OnSource_ContactEvent;

                _tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                _tableView.Source = source;
                _tableView.ReloadData();
            }
            else
            {
                _tableView.Hidden = true;
                _noContactsLabel.Hidden = false;
                _noContactsImage.Hidden = false;
            }
        }

        private void OnSource_ContactEvent(object sender, Tuple<ContactListViewModel.ContactEventType, int> contact)
        {
            _contactEvent?.Invoke(this, contact);
        }
    }
}

