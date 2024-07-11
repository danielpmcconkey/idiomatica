using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Logic.Telemetry;
using Model;

namespace Logic
{
    public static class NullHandler
    {
        public static string ThrowIfNullString(string? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return string.Empty;
            }
            return (string)input;
        }
        public static string ThrowIfNullOrEmptyString(string? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return string.Empty;
            }
            return (string)input;
        }
        public static T[] ThrowIfNullOrEmptyArray<T>(T[]? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Length == 0)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return [];
            }
            return (T[])input;
        }
        public static int ThrowIfNullInt(int? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return 0;
            }
            return (int)input;
        }
        public static int ThrowIfNullOrZeroInt(int? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input < 1)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return 0;
            }
            return (int)input;
        }
#nullable disable
        // nullable values turned off in this because a generic "key" value may be null
        // and you can't have a null key in a dictionary 
        public static Dictionary<TKey, TVal> ThrowIfNullDict<TKey, TVal>(Dictionary<TKey, TVal> input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new Dictionary<TKey, TVal>();
            }
            return (Dictionary<TKey, TVal>)input;
        }
        public static Dictionary<TKey, TVal> ThrowIfNullOrEmptyDict<TKey, TVal>(Dictionary<TKey, TVal> input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Count == 0)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new Dictionary<TKey, TVal>();
            }
            return (Dictionary<TKey, TVal>)input;
        }
#nullable restore
        public static List<T> ThrowIfNullOrEmptyList<T>(List<T>? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Count == 0)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new List<T>();
            }
            return (List<T>)input;
        }
        public static T ThrowIfNull<T>(T? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                ErrorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                return (T)input;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            return (T)input;
        }
    }
}
