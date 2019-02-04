using System;
using System.Collections.Generic;
using LetterApp.Models.DTO.ReceivedModels;
using UIKit;

namespace LetterApp.iOS.Models
{
    public class PositionsPickerModel : UIPickerViewModel
    {
        private List<PositionModel> _positions;
        private EventHandler<PositionModel> _selectedPosition;

        public PositionsPickerModel(List<PositionModel> positions, EventHandler<PositionModel> selectedPosition)
        {
            _positions = positions;
            _selectedPosition = selectedPosition;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component) => _positions[(int)row].Name;

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component) => (nint)_positions?.Count;

        public override nint GetComponentCount(UIPickerView pickerView) => 1;

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            _selectedPosition?.Invoke(this, _positions[(int)pickerView.SelectedRowInComponent(component)]);
        }
    }
}
