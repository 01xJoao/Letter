using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class DivisionCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DivisionCell");
        public static readonly UINib Nib;

        static DivisionCell()
        {
            Nib = UINib.FromName("DivisionCell", NSBundle.MainBundle);
        }

        protected DivisionCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
