using System;

namespace CommUtils.Logger
{
    public interface IJackyLogManager
    {
        void LogInfo(string s);
        void LogError(Exception exception);
    }
}