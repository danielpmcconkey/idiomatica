using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Telemetry
{
    public static class ErrorHandler
    {
        public static void LogAndThrow(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogError(memberName, sourceFilePath, sourceLineNumber, [], null);
            ThrowError(memberName, sourceFilePath, sourceLineNumber);
        }
        private static void LogError(
            string memberName, string sourceFilePath, int sourceLineNumber, string[] args, Exception? ex)
        {
            // todo: log errors
        }
        private static void ThrowError(string memberName, string sourceFilePath, int sourceLineNumber)
        {

            var ex = new IdiomaticaException()
            {
                memberName = memberName,
                sourceFilePath = sourceFilePath,
                sourceLineNumber = sourceLineNumber
            };
            throw ex;
        }
    }
}
