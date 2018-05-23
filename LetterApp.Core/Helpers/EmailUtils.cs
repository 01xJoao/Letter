using System;
using System.Net.Mail;

namespace LetterApp.Core.Helpers
{
    public static class EmailUtils
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
