// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LetterApp.iOS.Views.SelectPosition
{
    [Register ("SelectPositionViewController")]
    partial class SelectPositionViewController
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        UIKit.UIButton _buttonPicker { get; set; }


        [Outlet]
        UIKit.UIView _buttonPickerView { get; set; }


        [Outlet]
        UIKit.UIView _buttonView { get; set; }


        [Outlet]
        UIKit.UIView _loadingView { get; set; }


        [Outlet]
        UIKit.UIPickerView _picker { get; set; }


        [Outlet]
        UIKit.UIImageView _pickerImage { get; set; }


        [Outlet]
        UIKit.UILabel _pickerLabel { get; set; }


        [Outlet]
        UIKit.UIView _pickerView { get; set; }


        [Outlet]
        UIKit.UIButton _submitButton { get; set; }


        [Outlet]
        UIKit.UILabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_buttonPicker != null) {
                _buttonPicker.Dispose ();
                _buttonPicker = null;
            }

            if (_buttonPickerView != null) {
                _buttonPickerView.Dispose ();
                _buttonPickerView = null;
            }

            if (_buttonView != null) {
                _buttonView.Dispose ();
                _buttonView = null;
            }

            if (_loadingView != null) {
                _loadingView.Dispose ();
                _loadingView = null;
            }

            if (_picker != null) {
                _picker.Dispose ();
                _picker = null;
            }

            if (_pickerImage != null) {
                _pickerImage.Dispose ();
                _pickerImage = null;
            }

            if (_pickerLabel != null) {
                _pickerLabel.Dispose ();
                _pickerLabel = null;
            }

            if (_submitButton != null) {
                _submitButton.Dispose ();
                _submitButton = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}