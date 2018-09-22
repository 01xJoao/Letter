using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.Chat.Cells
{
    public partial class ImageWithUserCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ImageWithUserCell");
        public static readonly UINib Nib = UINib.FromName("ImageWithUserCell", NSBundle.MainBundle);
        protected ImageWithUserCell(IntPtr handle) : base(handle){}

        public void Configure()
        {
        }
    }
}
