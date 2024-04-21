
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Common.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    
        public static string SerializeToPlainString(this List<string> list, string delimiter = "@@")
        {
            var ret = string.Join(delimiter, list.ToArray());
            return ret;
        }
    
        public static List<string> DeserializePlainStringToList(this string src, string delimiter = "@@")
        {
            try
            {
                return src.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}