using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace sldataextractor.util.Extensions
{
    public static class StringExtensions
    {
        public static List<string> SplitIntoLines(this string inputString, int LineLength)
        {
            List<string> returnMe = new List<string>();

            int lastSeenSpace = 0;
            int lastEnding = 0;
            int lengthCounter = 0;
            for (int inputPosition = 0; inputPosition < inputString.Length; inputPosition++)
            {
                if (inputString[inputPosition] == ' ')
                {
                    lastSeenSpace = inputPosition;
                }

                if (lengthCounter >= LineLength)
                {
                    // Snip the string at the last space found
                    returnMe.Add(inputString.Substring(lastEnding, lastSeenSpace - lastEnding).Trim());
                    lastEnding = lastSeenSpace;

                    // Reset the length counter
                    lengthCounter = 0;
                }

                lengthCounter++;
            }

            // If we have any left over add it to the return
            returnMe.Add(inputString.Substring(lastEnding, inputString.Length - lastEnding).Trim());

            return returnMe;
        }


        public static List<string> SplidfghdfghtIntoLines(this string inputString, int LineLength)
        {
            List<string> returnMe = new List<string>();
            int stringPosition = 0;
            int inputStringLength = inputString.Length;

            while (stringPosition < inputStringLength)
            {
                int captureLength = LineLength;
                if (stringPosition + captureLength > inputStringLength)
                {
                    captureLength = inputStringLength - stringPosition;
                }

                // Nudge the cutoff to the left until we find a space character
                // so we don't cut off half of a word
                for (int x = stringPosition + captureLength; x < stringPosition; x--)
                {
                    if (inputString[x] == ' ')
                    {
                        captureLength -= x;
                        break;
                    }
                }

                returnMe.Add(inputString.Substring(stringPosition, captureLength));
                stringPosition = stringPosition + captureLength;
            }

            return returnMe;
        }

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
