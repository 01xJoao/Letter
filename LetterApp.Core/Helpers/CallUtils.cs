using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using Xamarin.Essentials;

namespace LetterApp.Core.Helpers
{
    public static class CallUtils
    {
        public static void Call(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }
    }
}
