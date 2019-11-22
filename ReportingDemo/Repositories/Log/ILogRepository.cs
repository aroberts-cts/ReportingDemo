using System;

namespace ReportingDemo.Repositories.Log
{
    public interface ILogRepository
    {
        void Info(string msg, string memberName = "", int lineNumber = 0);
        void Warn(string msg);
        void Warn(string msg, Exception e, string memberName = "", int lineNumber = 0);
        void Error(string msg);
        void Error(string msg, Exception e, string memberName = "", int lineNumber = 0);
    }
} 