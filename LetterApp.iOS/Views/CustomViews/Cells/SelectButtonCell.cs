using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.CustomViews.Cells
{
    public partial class SelectButtonCell : UITableViewCell
    {
        public EventHandler OnSubmitButton;
        public static readonly NSString Key = new NSString("SelectButtonCell");
        public static readonly UINib Nib = UINib.FromName("SelectButtonCell", NSBundle.MainBundle);
        protected SelectButtonCell(IntPtr handle) : base(handle) {}
        public void Configure(string text, EventHandler eventHandler) {}
    }
}
