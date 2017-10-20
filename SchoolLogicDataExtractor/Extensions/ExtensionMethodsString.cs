using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    public static class ExtensionMethodsString
    {
        public static string RemoveSpecialCharacters(this string inputString)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder returnMe = new StringBuilder();

            foreach (char c in inputString)
            {
                if (allowedChars.Contains(c))
                {
                    returnMe.Append(c);
                }
            }

            return returnMe.ToString();
        }

        public static string ToSingleLine(this string inputString)
        {
            return inputString.Trim().Replace("\n", ", ").Replace("\r", "").Trim();
        }

        /// <summary>
        /// Sets the length of the string to a specific length, removing characters and adding spaces as required
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width">How long should the string be</param>
        /// <returns></returns>
        public static string SetLength(this string value, int width)
        {
            return value.Length > width ? value.Substring(0, width) : value.PadRight(width);
        }

        /// <summary>
        /// Sets the length of the string to a specific length, removing characters and adding spaces as required
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width">How long should the string be</param>
        /// <param name="padCharacter">Character to use for padding</param>
        /// <returns></returns>
        public static string SetLength(this string value, int width, char padCharacter)
        {
            return value.Length > width ? value.Substring(0, width) : value.PadRight(width, padCharacter);
        }

        public static string NoLongerThan(this string value, int width)
        {
            return value.Length > width ? value.Substring(0, width) : value;
        }

        public static string RemoveSpaces(this string working)
        {
            try
            {
                return Regex.Replace(working, @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string FixedWidthString(this string value, int width)
        {
            return value.Length > width ? value.Substring(0, width) : value.PadRight(width);
        }
    }
}
