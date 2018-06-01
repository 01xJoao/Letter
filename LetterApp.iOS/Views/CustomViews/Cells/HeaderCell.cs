using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class HeaderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderCell");
        public static readonly UINib Nib = UINib.FromName("HeaderCell", NSBundle.MainBundle);
        protected HeaderCell(IntPtr handle) : base(handle) {}


        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            _imageView.Image = UIImage.FromBundle("letter_round_big");
        }
    }
}
