using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sldataextractor.util.Extensions
{
    public static class ListExtensions
    {

        /// <summary>
        /// Converts a list of integers to a comma seperated string, for displaying somewhere to the user
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToCommaSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToCommaSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToSemicolenSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static string ToSemicolenSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static List<string> SortByGrade(this List<string> list)
        {
            List<string> returnMe = new List<string>();

            if (list.Contains("PK"))
            {
                returnMe.Add("PK");
            }

            if (list.Contains("0K"))
            {
                returnMe.Add("K");
            }

            if (list.Contains("K"))
            {
                returnMe.Add("K");
            }

            // Parse the rest into Ints and sort them
            List<int> parsed = new List<int>();
            foreach (string li in list.Where(i => i != "PK" && i != "0K" && i != "K"))
            {
                parsed.Add(Parsers.ParseInt(li));
            }

            foreach (int i in parsed.OrderBy(i => i))
            {
                returnMe.Add(i.ToString());
            }
            return returnMe;
        }


        public static void AddRangeUnique<T>(this List<T> thisList, List<T> collection)
        {
            foreach (T potentialNewItem in collection.Where(potentialNewItem => !thisList.Contains(potentialNewItem)))
            {
                thisList.Add(potentialNewItem);
            }
        }

        public static void AddUnique<T>(this List<T> thisList, T obj)
        {
            if (!thisList.Contains(obj))
            {
                thisList.Add(obj);
            }
        }

        public static string ToCommaSeparatedString<T>(this List<T> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (T item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToLineBreakSeparatedString<T>(this List<T> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (T item in list)
            {
                returnMe.Append(item);
                returnMe.Append("\n");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }


    }
}
