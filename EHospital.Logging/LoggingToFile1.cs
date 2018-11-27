using System;

namespace EHospital.Logging
{
    public static class LoggingToFile
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LoggingInfo(string message)
        {
            log.Info(message);
        }

        public static void LoggingWarn(string message)
        {
            log.Warn(message);
        }

        public static void LoggingWarn(string message, Exception ex)
        {
            log.Warn(message, ex);
        }

        public static void LoggingError(string message)
        {
            log.Error(message);
        }

        public static void LoggingError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
