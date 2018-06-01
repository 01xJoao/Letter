using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LetterApp.Core.Helpers
{
    public static class StringUtils
    {
        private static string _specialChars = "<>@!#$%^&*()_+[]{}?:;|'\"\\,./~`-=";

        public static bool IsLettersOnly(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            
            for (int i = 0; i < _specialChars.Length; i++)
            {
                if (str?.IndexOf(_specialChars[i]) > -1)
                    return false;
            }

            return true;
        }

        public static bool IsDigitsOnly(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            
            return str.Any(ch => Char.IsDigit(ch));
        }

        public static string RemoveWhiteSpaces(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            
            return string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
        }

        public static bool CheckForEmojis(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            
            var new_str = Regex.Replace(str, @"\p{Cs}", "");
            return str == new_str;
        }
    }
}
