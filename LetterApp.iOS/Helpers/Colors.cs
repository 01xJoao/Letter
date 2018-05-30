using System;
using UIKit;

namespace LetterApp.iOS.Helpers
{
    public static class Colors
    {
        public static UIColor MainBlue => UIColor.FromRGB(38, 95, 135);                         //#207CC5
        public static UIColor MainBlue4 => UIColor.FromRGB(247, 249, 250);                      //#207CC5
        public static UIColor TabBarNotSelected => UIColor.FromRGB(181, 181, 181);              //#207CC5
        public static UIColor White => UIColor.FromRGB(255, 255, 255);                          //#FFFFFF
        public static UIColor Black => UIColor.FromRGB(0, 0, 0);                                //#000000
        public static UIColor Black30 => UIColor.FromRGB(0, 0, 0).ColorWithAlpha(0.3f);         //#000000
        public static UIColor BlueButtonPressed => UIColor.FromRGB(32, 147, 196);               //#2093C4
        public static UIColor GrayIndicator => UIColor.FromRGB(158, 158, 158);                  //#9B9E9E
        public static UIColor GrayDivider => UIColor.FromRGB(177, 181, 181);                    //#B1B5B5
        public static UIColor Red => UIColor.FromRGB(219, 82, 74);                              //#DB524A
        public static UIColor Green => UIColor.FromRGB(54, 180, 114);                           //#36B472
        public static UIColor Orange => UIColor.FromRGB(255, 161, 2);                           //#FFA102
    }
}
