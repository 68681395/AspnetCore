using System;

namespace Common.Logging
{
    using Microsoft.Extensions.Logging;

    internal class LogManager
    {
        private static ILoggerFactory factory;
        static LogManager()
        {
             factory = new LoggerFactory()
              .WithFilter(new FilterLoggerSettings
              {
                    { "Microsoft", LogLevel.Warning },
                    { "System", LogLevel.Warning },
                    { "SampleApp.Program", LogLevel.Debug }
              });

            // getting the logger immediately using the class's name is conventional
            
        }
        internal static ILog GetCurrentClassLogger()
        {
         


            return new Log();
        }

        internal static ILog GetLogger(string v)
        {
            return new Log();
        }
    }

    public interface ILog
    {
        void Error(string v, Exception ex);
        void Warn(string v, Exception ex);
        void Debug(string v);
        void Warn(string v);
        void ErrorFormat(string v, Exception ex, params object[] args);
        void Error(Exception ex);
    }

    class Log : ILog
    {
        //private Microsoft.Extensions.Logging.Logger<string> logger = new Microsoft.Extensions.Logging.Logger<string>();


        public void Debug(string v)
        {
            
        }

        public void Error(Exception ex)
        {
             
        }

        public void Error(string v, Exception ex)
        {
            
        }

        public void ErrorFormat(string v, Exception ex, params object[] args)
        {
            
        }

        public void Warn(string v)
        {
             
        }

        public void Warn(string v, Exception ex)
        {
             
        }
    }
}