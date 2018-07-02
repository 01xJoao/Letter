﻿using System;

using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.SelectDivision.Cells
{
    public partial class LineSeparatorCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("LineSeparatorCell");
        public static readonly UINib Nib = UINib.FromName("LineSeparatorCell", NSBundle.MainBundle);
        protected LineSeparatorCell(IntPtr handle) : base(handle) {}
    }
}
