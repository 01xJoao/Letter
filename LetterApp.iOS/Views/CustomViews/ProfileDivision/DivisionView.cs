using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.ProfileDivision
{
    [Register("DivisionView")]
    public partial class DivisionView : UIView
    {
        private static EventHandler<int> _divisionEvent;
        private static Core.Models.ProfileDivision _division;
        public static readonly UINib Nib = UINib.FromName("DivisionView", NSBundle.MainBundle);
        public DivisionView(IntPtr handle) : base(handle) {}

        public static DivisionView Create(Core.Models.ProfileDivision division, EventHandler<int> divisionEvent)
        {
            _division = division;
            _divisionEvent = divisionEvent;
            return Nib.Instantiate(null, null)[0] as DivisionView;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _imageView.Image?.Dispose();

            if (_division.AddButtonImage)
            {
                _imageView.Image = UIImage.FromBundle("add_circle");
                _label.Hidden = true;
            }
            else
            {
                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _division.Picture);
                }).LoadingPlaceholder("warning_image", ImageSource.CompiledResource).Transform(new CircleTransformation()).Into(_imageView);

                _label.Hidden = false;
                UILabelExtensions.SetupLabelAppearance(_label, _division.Name, Colors.ProfileGray, 13f);
            }

            _button.TouchUpInside -= OnButton_TouchUpInside;
            _button.TouchUpInside += OnButton_TouchUpInside;
        }

        private void OnButton_TouchUpInside(object sender, EventArgs e)
        {
            _divisionEvent?.Invoke(this, _division.Id);
        }
    }
}
