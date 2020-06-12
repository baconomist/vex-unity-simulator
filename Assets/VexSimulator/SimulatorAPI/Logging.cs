using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using VexSimulator.SimulatorAPI.UnsafeCppAPI;

namespace VexSimulator.SimulatorAPI
{
    public static class Logging
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void LoggingCallback();

        public enum LogLevel
        {
            None = 0,
            Info = 1,
            Debug = 2,
            Warning = 3,
            Error = 4,
            Exception = 5
        }

        public static LogLevel loggingLevel = LogLevel.Exception;

        private static LoggingCallback logInfoCallback;
        private static LoggingCallback logDebugCallback;
        private static LoggingCallback logWarnCallback;
        private static LoggingCallback logErrCallback;
        private static LoggingCallback logExceptCallback;

        private static void LogCPPBuffer(LogLevel logLevel)
        {
            APIMethods.RequireAPIInitialized();

            int outputBufferSize = UnsafeCppAPI.UnsafeLogging.GetOutputBufferSize();
            StringBuilder buffer = new StringBuilder(outputBufferSize);
            UnsafeCppAPI.UnsafeLogging.ReadOutputBuffer(buffer);

            if ((logLevel == LogLevel.Info || logLevel == LogLevel.Debug) && loggingLevel != LogLevel.None)
                Debug.Log(buffer);
            else if (logLevel == LogLevel.Warning && loggingLevel >= LogLevel.Warning)
                Debug.LogWarning(buffer);
            else if ((logLevel == LogLevel.Error || logLevel == LogLevel.Exception) && loggingLevel >= LogLevel.Error)
                Debug.LogError(buffer);
        }

        //https://stackoverflow.com/questions/39790977/how-to-pass-a-delegate-or-function-pointer-from-c-sharp-to-c-and-call-it-there
        public static void SetupLogHandlers()
        {
            APIMethods.RequireAPIInitialized();

            logInfoCallback = () => LogCPPBuffer(LogLevel.Info);
            logDebugCallback = () => LogCPPBuffer(LogLevel.Debug);
            logWarnCallback = () => LogCPPBuffer(LogLevel.Warning);
            logErrCallback = () => LogCPPBuffer(LogLevel.Error);
            logExceptCallback = () => LogCPPBuffer(LogLevel.Exception);

            UnsafeCppAPI.UnsafeLogging.SetLogInfoListener(Marshal.GetFunctionPointerForDelegate(logInfoCallback));
            UnsafeCppAPI.UnsafeLogging.SetLogDebugListener(Marshal.GetFunctionPointerForDelegate(logDebugCallback));
            UnsafeCppAPI.UnsafeLogging.SetLogWarnListener(Marshal.GetFunctionPointerForDelegate(logWarnCallback));
            UnsafeCppAPI.UnsafeLogging.SetLogErrListener(Marshal.GetFunctionPointerForDelegate(logErrCallback));
            UnsafeCppAPI.UnsafeLogging.SetLogExceptListener(Marshal.GetFunctionPointerForDelegate(logExceptCallback));
        }
    }
}