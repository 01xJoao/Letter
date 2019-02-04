using System;
using System.Collections.Generic;
using LetterApp.Core;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Dialog
{
    public partial class FilterContactsViewController : UIViewController
    {
        private string _title;
        private string _descriptionText;
        private string _buttonText;
        private List<ContactDialogFilter> _filters;
        private Action<Tuple<bool,bool>> _buttonEvent;

        public FilterContactsViewController(string title, List<ContactDialogFilter> filters, string buttonText, Action<Tuple<bool,bool>> button) : base("FilterContactsViewController", null)
        {
            _title = title;
            _filters = filters;
            _buttonText = buttonText;
            _buttonEvent = button;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.Alpha = 0.3f;

            this.View.BackgroundColor = Colors.Black30;
            _backGroundView.Layer.CornerRadius = 2f;
            _buttonColorBig.Layer.CornerRadius = 2f;
            CustomUIExtensions.ViewShadow(_backGroundView);

            UILabelExtensions.SetupLabelAppearance(_titleLabel, _title, Colors.Black, 24f, UIFontWeight.Medium);

            UILabelExtensions.SetupLabelAppearance(_filterByNameLabel, _filters[0].Title, Colors.Black, 14f);
            UILabelExtensions.SetupLabelAppearance(_filterByNameDescription, _filters[0].Description, Colors.GrayLabel, 12f);

            UILabelExtensions.SetupLabelAppearance(_switchLabel, _filters[1].Title, Colors.Black, 14f);
            UILabelExtensions.SetupLabelAppearance(_descriptionLabel, _filters[1].Description, Colors.GrayLabel, 12f);

            _buttonColorBig.BackgroundColor = Colors.MainBlue;
            _buttonColorSmall.BackgroundColor = Colors.MainBlue;
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.White, 17f, _buttonText);

            _closeButton.SetImage(UIImage.FromBundle("close_black"), UIControlState.Normal);
            _closeButton.ContentMode = UIViewContentMode.ScaleAspectFit;
            _closeButton.TintColor = Colors.Black;

            _filterByNameSwitch.On = _filters[0].IsActive;
            _switch.On = _filters[1].IsActive;

            _closeButton.TouchUpInside -= OnCloseButton_TouchUpInside;
            _closeButton.TouchUpInside += OnCloseButton_TouchUpInside;

            _button.TouchUpInside -= OnSubmitButton_TouchUpInside;
            _button.TouchUpInside += OnSubmitButton_TouchUpInside;
        }

        private void OnSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(new Tuple<bool, bool>(_filterByNameSwitch.On, _switch.On));
            Dismiss();
        }

        private void OnCloseButton_TouchUpInside(object sender, EventArgs e)
        {
            _buttonEvent?.Invoke(new Tuple<bool, bool>(_filters[0].IsActive, _filters[1].IsActive));
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
            MemoryUtility.ReleaseUIViewWithChildren(this.View);
        }
    }
}

