using System;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Views.TabBar.CallListViewController.Cells
{
    public partial class CallHistoryCell : UITableViewCell
    {
        private string _picture;
        private int _callerId;
        private EventHandler<int> _openProfile;

        public static readonly NSString Key = new NSString("CallHistoryCell");
        public static readonly UINib Nib = UINib.FromName("CallHistoryCell", NSBundle.MainBundle);

        protected CallHistoryCell(IntPtr handle) : base(handle) {}

        public void Configure(CallHistoryModel call, EventHandler<int> openProfile)
        {
            _callerId = call.CallerId;
            _openProfile = openProfile;

            var nameAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.ProfileGray,
                Font = UIFont.SystemFontOfSize(14, call.ShouldAlert ? UIFontWeight.Bold : UIFontWeight.Medium)
            };

            var RoleAttr = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.SystemFontOfSize(14, call.ShouldAlert ? UIFontWeight.Semibold : UIFontWeight.Regular)
            };

            var letterCount = call.CallerInfo.IndexOf("·");

            var customString = new NSMutableAttributedString(call.CallerInfo);
            customString.SetAttributes(nameAttr.Dictionary, new NSRange(0, letterCount));
            customString.SetAttributes(RoleAttr.Dictionary, new NSRange(letterCount + 1, call.CallerInfo.Length - (letterCount + 1)));

            // assign the styled text
            _callerInfoLabel.AttributedText = customString;

            UILabelExtensions.SetupLabelAppearance(_dateLabel, call.CallDateText, Colors.ProfileGray, 13f, call.ShouldAlert ? UIFontWeight.Semibold : UIFontWeight.Regular);
            UILabelExtensions.SetupLabelAppearance(_callingTypeLabel, call.CallCountAndType, call.HasSuccess ? Colors.Green : Colors.Red, 13f, call.ShouldAlert ? UIFontWeight.Semibold : UIFontWeight.Regular);

            _imageView.Image?.Dispose();

            if (!string.IsNullOrEmpty(call.CallerPicture))
            {
                _picture = string.Copy(call.CallerPicture);

                ImageService.Instance.LoadStream((token) => {
                    return ImageHelper.GetStreamFromImageByte(token, _picture);
                }).ErrorPlaceholder("profile_noimage", ImageSource.CompiledResource).Retry(3, 200).Finish(CleanString).Transform(new CircleTransformation()).Into(_imageView);
            }
            else
            {
                _imageView.Image = UIImage.FromBundle("profile_noimage");
                CustomUIExtensions.RoundView(_imageView);
            }

            _openProfileButton.TouchUpInside -= OnOpenProfileButton_TouchUpInside;
            _openProfileButton.TouchUpInside += OnOpenProfileButton_TouchUpInside;
        }

        private void CleanString(IScheduledWork obj)
        {
            _picture = null;
        }

        private void OnOpenProfileButton_TouchUpInside(object sender, EventArgs e)
        {
            _openProfile?.Invoke(sender, _callerId);
        }
    }
}
