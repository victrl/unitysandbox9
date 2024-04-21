
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Common.Extensions
{
    public static class EnumExtensions
    {
        public static Array ConvertToArray(this Enum type)
        {
            var t = type.GetType();
            Array ret = Enum.GetValues(t);
            return ret;
        }
    
        public static IEnumerable<T> GetValuesAs<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}