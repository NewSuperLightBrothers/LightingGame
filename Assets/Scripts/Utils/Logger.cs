using UnityEngine;

using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public static class Logger
    {
        // Add "ENABLE_LOGS" string
        // in [Project Setting] - [ Other Setting ] - [Scripting Define Symbols]
        private const string _ENABLE_LOGS = "ENABLE_LOGS";
        private const string _IDENTIFIER = "LightGame";

        public static bool isDebugBuild => Debug.isDebugBuild;

        private static string GetPrefix()
        {            
            string prevFuncName = new StackFrame(2, true).GetMethod().Name;
            string prevClassName = new StackTrace().GetFrame(2).GetMethod().ReflectedType?.Name;
            return $"{_IDENTIFIER}-[{prevClassName} / {prevFuncName}] : ";
        }

        #region Log
        
        [Conditional(_ENABLE_LOGS)]
        public static void Log(object message)
            => Debug.Log($"{GetPrefix()}{message}");

        [Conditional(_ENABLE_LOGS)]
        public static void Log(object message, Object context)
            => Debug.Log($"{GetPrefix()}{message}", context);
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogFormat(string message, params object[] args)
            => Debug.LogFormat($"{GetPrefix()}{message}", args);

        [Conditional(_ENABLE_LOGS)]
        public static void LogFormat(Object context, string message, params object[] args)
            => Debug.LogFormat(context, $"{GetPrefix()}{message}", args);

        #endregion

        #region Logwarning
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogWarning(object message)
            => Debug.LogWarning($"{GetPrefix()}{message}");

        [Conditional(_ENABLE_LOGS)]
        public static void LogWarning(object message, Object context)
            => Debug.LogWarning($"{GetPrefix()}{message}", context);

        [Conditional(_ENABLE_LOGS)]
        public static void LogWarningFormat(string message, params object[] args)
            => Debug.LogWarningFormat($"{GetPrefix()}{message}", args);
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogWarningFormat(Object context, string message, params object[] args)
            => Debug.LogWarningFormat(context, $"{GetPrefix()}{message}", args);
        
        #endregion

        #region LogError
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogError(object message)
            => Debug.LogError($"{GetPrefix()}{message}");

        [Conditional(_ENABLE_LOGS)]
        public static void LogError(object message, Object context)
            => Debug.LogError($"{GetPrefix()}{message}", context);
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogErrorFormat(string message, params object[] args)
            => Debug.LogErrorFormat($"{GetPrefix()}{message}", args);

        [Conditional(_ENABLE_LOGS)]
        public static void LogErrorFormat(Object context, string message, params object[] args)
            => Debug.LogErrorFormat(context, $"{GetPrefix()}{message}", args);

        #endregion

        #region LogException
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogException(System.Exception exception)
            => Debug.LogException(exception);
        
        [Conditional(_ENABLE_LOGS)]
        public static void LogException(System.Exception exception, Object context)
            => Debug.LogException(exception, context);

        #endregion
    }
}