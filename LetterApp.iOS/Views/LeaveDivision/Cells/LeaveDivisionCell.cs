using System;

using Foundation;
using LetterApp.iOS.Helpers;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Views.LeaveDivision.Cells
{
    public partial class LeaveDivisionCell : UITableViewCell
    {
        private NSIndexPath _indexPath;
        private DivisionModel _division;
        private EventHandler<Tuple<NSIndexPath, DivisionModel>> _leaveDivision;
        public static readonly NSString Key = new NSString("LeaveDivisionCell");
        public static readonly UINib Nib = UINib.FromName("LeaveDivisionCell", NSBundle.MainBundle);
        protected LeaveDivisionCell(IntPtr handle) : base(handle) {}

        public void Configure(DivisionModel division, string leave, EventHandler<Tuple<NSIndexPath, DivisionModel>> leaveDivision, NSIndexPath indexPath)
        {
            _indexPath = indexPath;
            _division = division;
            _leaveDivision = leaveDivision;
            UILabelExtensions.SetupLabelAppearance(_label, division.Name, Colors.Black, 16f);
            UIButtonExtensions.SetupButtonAppearance(_button, Colors.Red, 15f, leave);

            _button.TouchUpInside -= OnButon_TouchUpInside;
            _button.TouchUpInside += OnButon_TouchUpInside;
        }

        private void OnButon_TouchUpInside(object sender, EventArgs e)
        {
            _leaveDivision?.Invoke(this, new Tuple<NSIndexPath, DivisionModel>(_indexPath, _division));
        }
    }
}
