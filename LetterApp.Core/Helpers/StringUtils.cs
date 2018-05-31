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
            for (int i = 0; i < _specialChars.Length; i++)
            {
                if (str?.IndexOf(_specialChars[i]) > -1)
                    return false;
            }

            return true;
        }

        public static bool IsDigitsOnly(string str)
        {
            return str.Any(ch => Char.IsDigit(ch));
        }

        public static string RemoveWhiteSpaces(string str)
        {
            return string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
        }

        public static bool CheckForEmojis(string str)
        {
            var new_str = Regex.Replace(str, @"\p{Cs}", "");
            return str == new_str;
        }
    }
}
