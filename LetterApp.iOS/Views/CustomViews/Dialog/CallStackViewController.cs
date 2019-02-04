using System;
using System.Collections.Generic;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Sources;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class CallStackViewController : UIViewController
    {
        private string _title;
        private List<CallStackModel> _calls;

        public CallStackViewController(string title, List<CallStackModel> calls) : base("CallStackViewController", null)
        {
            _title = title;
            _calls = calls;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;
            this.View.BackgroundColor = Colors.Black30;
            _backgroundView.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backgroundView);

            _separatorView.BackgroundColor = Colors.AlertDividerColor;

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.Black, 18f, UIFontWeight.Medium);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.ContentMode = UIViewContentMode.ScaleAspectFit;
            _closeButton.TintColor = Colors.Black;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            SetupTableView();
        }

        private void SetupTableView()
        {
            _tableView.Source = new CallStackSource(_calls);
            _tableView.ReloadData();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            Dismiss();
        }

        public void Show()
        {
            this.View.Frame = UIApplication.SharedApplication.KeyWindow.Bounds;
            UIApplication.SharedApplication.KeyWindow.AddSubview(this.View);
            UIView.Animate(0.3, () => View.Alpha = 1);
        }

        public void Dismiss()
        {
            UIView.AnimateNotify(0.3, () => View.Alpha = 0, (finished) => CleanFromMemory());
        }

        private void CleanFromMemory()
        {
            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

