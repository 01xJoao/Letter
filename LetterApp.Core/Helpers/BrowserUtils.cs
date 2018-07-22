using System;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;

namespace LetterApp.Core.Helpers
{
    public static class BrowserUtils
    {
        public static async Task OpenWebsite(string link)
        {
            try
            {
                await Xamarin.Essentials.Browser.OpenAsync(link);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
        }
    }
}
