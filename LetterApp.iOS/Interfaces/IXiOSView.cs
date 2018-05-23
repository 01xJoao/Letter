using System;
namespace LetterApp.iOS.Interfaces
{
    public interface IXiOSView
    {
        object ParameterData { get; set; }
        bool ShowAsPresentView { get; }
    }
}
