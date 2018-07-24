using System;
using System.Collections.Generic;
using LetterApp.Core.Models;
using LetterApp.iOS.Views.Base;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.ContactListViewController.PageViewController
{
    public partial class ContactPageViewController : XBoardPageViewController
    {
        private int _index;
        private List<GetUsersInDivisionModel> _contactPage;

        public ContactPageViewController(int index, List<GetUsersInDivisionModel> contactPage) : base(index, "ContactPageViewController", null)
        {
            _index = index;
            _contactPage = contactPage;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

