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
    public class NullHandler
    {
        ErrorHandler _errorHandler;
        public NullHandler(ErrorHandler errorHandler)
        { 
            _errorHandler = errorHandler;
        }
        public string ThrowIfNullString(string? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return string.Empty;
            }
            return (string)input;
        }
        public string ThrowIfNullOrEmptyString(string? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return string.Empty;
            }
            return (string)input;
        }
        public T[] ThrowIfNullOrEmptyArray<T>(T[]? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Length == 0)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return [];
            }
            return (T[])input;
        }
        public int ThrowIfNullInt(int? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return 0;
            }
            return (int)input;
        }
        public int ThrowIfNullOrZeroInt(int? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input < 1)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return 0;
            }
            return (int)input;
        }
#nullable disable
        // nullable values turned off in this because a generic "key" value may be null
        // and you can't have a null key in a dictionary 
        public Dictionary<TKey, TVal> ThrowIfNullDict<TKey, TVal>(Dictionary<TKey, TVal> input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new Dictionary<TKey, TVal>();
            }
            return (Dictionary<TKey, TVal>)input;
        }
        public Dictionary<TKey, TVal> ThrowIfNullOrEmptyDict<TKey, TVal>(Dictionary<TKey, TVal> input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Count == 0)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new Dictionary<TKey, TVal>();
            }
            return (Dictionary<TKey, TVal>)input;
        }
#nullable restore
        public List<T> ThrowIfNullOrEmptyList<T>(List<T>? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null || input.Count == 0)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
                return new List<T>();
            }
            return (List<T>)input;
        }
        public T ThrowIfNull<T>(T? input,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (input == null)
            {
                _errorHandler.LogAndThrow(memberName, sourceFilePath, sourceLineNumber);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                return (T)input;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            return (T)input;
        }
    }
}
