
using System;

namespace App.Common.Tools
{
    public static class LoggerExtensions
    {
        public static string GetInfo(this Exception exception)
        {
            return $"\n Exception: Message - {exception.Message} \n StackTrace - {exception.StackTrace}";
        }
    }
}