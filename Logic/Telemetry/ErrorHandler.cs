//#define LOGDEBUG // when set, all log message type will be written to the DB
using Microsoft.Extensions.Logging;
using Model.DAL;
using Model.Enums;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.EntityFrameworkCore;

namespace Logic.Telemetry
{
    public static class ErrorHandler
    {
        public static void LogAndThrow(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = "Error thrown in application.";
            LogMessage(AvailableLogMessageTypes.ERROR,
                memberName, sourceFilePath, sourceLineNumber, message, null, null);
            ThrowError(memberName, sourceFilePath, sourceLineNumber);
        }
        public static void LogMessage(
            AvailableLogMessageTypes type, string message, 
            IDbContextFactory<IdiomaticaContext> dbContextFactory = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            LogMessage(type, memberName, sourceFilePath, sourceLineNumber,
                message, null, dbContextFactory);
        }
        private static void LogMessage(AvailableLogMessageTypes type,
            string memberName, string sourceFilePath, int sourceLineNumber, string message,
            Exception? ex, IDbContextFactory<IdiomaticaContext> dbContextFactory = null)
        {
            if (dbContextFactory is null) return;

            var shouldWriteLog = false;
            if (type == AvailableLogMessageTypes.FATAL || type == AvailableLogMessageTypes.ERROR
                 || type == AvailableLogMessageTypes.WARNING)
            {
                shouldWriteLog = true;
            }
#if DEBUG
            if (type == AvailableLogMessageTypes.MESSAGE) shouldWriteLog = true;
#endif
#if LOGDEBUG
            shouldWriteLog = true;
#endif
            if (!shouldWriteLog) return;

            var context = dbContextFactory.CreateDbContext();

            context.LogMessages.Add(new LogMessage() { 
                Id = Guid.NewGuid(),
                Logged = DateTimeOffset.Now,
                MessageType = type,
                Message = message,
                Detail = ex?.ToString(),
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber
            });
            context.SaveChanges();
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
