using System;
using Foundation;
using UIKit;

namespace LetterApp.iOS.Views.Base
{
    [Register("XBoardPageViewController")]
    public abstract class XBoardPageViewController : UIViewController
    {
        public int Index;

        public XBoardPageViewController(int index, string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            Index = index;
        }
    }
}
