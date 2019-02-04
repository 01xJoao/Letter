using System;
using System.Linq;
namespace LetterApp.iOS.Helpers
{
    public static class PhoneModelExtensions
    {
        public static bool IsSmallIphone()
        {
            var model = Xamarin.iOS.DeviceHardware.Model.ToLower();

            if (model.Contains("5") || model.Contains("se"))
                return true;

            return false;
        }

        public static bool IsIphoneX()
        {
            var model = Xamarin.iOS.DeviceHardware.Model.ToLower();

            if (model.Contains("x"))
                return true;

            return false;
        }
    }
}
