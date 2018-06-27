using System;
using System.Net.Mail;

namespace LetterApp.Core.Helpers
{
    public static class EmailUtils
    {

        private static string[] publicDomain = { "gmail", "outlook", "icloud", "yahoo", "zoho", 
                                                 "hotmail", "mailinator", "maildrop", "fastmail", 
                                                 "protonmail", "tutanota", "aol", "hushmail", "gmx"};

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

        public static string GetEmailDomain(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return IsPublicDomain(address.Host.Split('.')[0]);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string IsPublicDomain(string domain)
        {
            foreach(string pubDomain in publicDomain)
            {
                if (pubDomain == domain)
                    return string.Empty;
            }
            return domain;
        }
    }
}
