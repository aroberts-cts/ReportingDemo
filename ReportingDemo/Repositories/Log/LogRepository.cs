using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using log4net.Appender;

namespace ReportingDemo.Repositories.Log
{
    public class LogRepository : ILogRepository
    {
        private static readonly IDictionary<Type, ILog> _loggersByType = new Dictionary<Type, ILog>();
        private static readonly object _lock = new Object(); // Lock can't be null

        public static LogRepository GetLogger<T>() where T : class
        {
            var classType = typeof(T);
            ILog logger = null;

            // Try once outside the synchronization to avoid the cost of the lock
            if (_loggersByType.ContainsKey(classType))
            {
                logger = _loggersByType[classType];
                return new LogRepository(logger);
            }

            lock (_lock)
            {
                // Now make sure it wasn't due to the dictionary being out of sync
                if (_loggersByType.ContainsKey(classType))
                {
                    logger = _loggersByType[classType];
                    return new LogRepository(logger);
                }

                // Now that we're certain it doesn't exist, create a new one and cache it
                logger = LogManager.GetLogger(classType);
                _loggersByType.Add(classType, logger);

                return new LogRepository(logger);
            }
        }

        public ILog Log { get; }

        private LogRepository(ILog log)
        {
            Log = log;
        }

        public static ILogRepository Logger(Type type)
        {
            return new LogRepository(LogManager.GetLogger(type));
        }

        public void Info(string msg,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log.Info(FormatMessage(msg, memberName, lineNumber));
        }

        public void Warn(string msg)
        {
            Log.Warn(FormatMessage(msg, string.Empty, 0));
        }

        public void Warn(string msg, Exception e,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log.Warn(FormatMessage(msg, memberName, lineNumber, e));
        }

        [Obsolete("Please pass the exception object through to get properly formatted exception logging")]
        public void Error(string msg)
        {
            Log.Error(FormatMessage(msg, string.Empty, 0));
        }

        public void Error(string msg, Exception e,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            Log.Error(FormatMessage(msg, memberName, lineNumber, e));
        }

        private string FormatMessage(string msg, string memberName, int lineNumber, Exception e = null)
        {
            var requestId = OperationContextExtension.Current.RequestId;

            var formattedLogMessage = $"{{{Environment.NewLine}"
                + $"  Request: {$@"{requestId}"},{Environment.NewLine}"
                + $"  Calling Member: {$@"{memberName}"},{Environment.NewLine}"
                + $"  Line Number: {$@"{lineNumber}"},{Environment.NewLine}"
                + $"  Message: {$@"{msg}"}";

            if (e != null)
            {
                formattedLogMessage +=
                    $",{Environment.NewLine}  Exception: {$@"{e.Message}"},{Environment.NewLine}  StackTrace: {$@"{e.StackTrace}"}";
            }

            return formattedLogMessage;
        }

        private void Flush()
        {
            if (!Debugger.IsAttached) return;

            foreach (var appender in LogManager.GetRepository().GetAppenders().OfType<AdoNetAppender>()) appender.Flush();
        }
    }
}