using System;

namespace Common.Logging
{
    internal class LogManager
    {
        internal static ILog GetCurrentClassLogger()
        {
            throw new NotImplementedException();
        }

        internal static ILog GetLogger(string v)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILog
    {
        void Error(string v, Exception ex);
        void Warn(string v, Exception ex);
        void Debug(string v);
        void Warn(string v);
        void ErrorFormat(string v, Exception ex, params object[] args);
        void Error(TypeInitializationException ex);
    }

}