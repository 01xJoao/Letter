using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class InsertDivisionFieldCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("InsertDivisionFieldCell");
        public static readonly UINib Nib;

        static InsertDivisionFieldCell()
        {
            Nib = UINib.FromName("InsertDivisionFieldCell", NSBundle.MainBundle);
        }

        protected InsertDivisionFieldCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
