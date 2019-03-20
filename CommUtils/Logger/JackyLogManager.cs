using System;
using NLog;

namespace CommUtils.Logger
{
    public class JackyLogManager : IJackyLogManager
    {
        private NLog.Logger _logger;

        public JackyLogManager()
        {
            _logger = LogManager.GetLogger("JackyLogger");
        }

        public void LogInfo(string s)
        {
            _logger.Info(s);
        }

        public void LogError(Exception exception)
        {
            _logger.Error(exception);
        }
    }
}