using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityNativeTool;
using VexSimulator.SimulatorAPI.CppAPI;

namespace VexSimulator.SimulatorAPI
{
    public class SimulatorAPI : MonoBehaviour
    {
        public static bool robotInitialized = false;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void LoggingCallback();

        private enum LogLevel
        {
            Info,
            Debug,
            Warning,
            Error,
            Exception
        }

        private static LoggingCallback logInfoCallback;
        private static LoggingCallback logDebugCallback;
        private static LoggingCallback logWarnCallback;
        private static LoggingCallback logErrCallback;
        private static LoggingCallback logExceptCallback;

        public static void ReloadAPI()
        {
            if(CppAPIMethods.IsAPIInitialized() == 1) DestroyAPI();
            Debug.Log("Unity CPP API Reloading, IsAPIInitialized() value: " + CppAPIMethods.IsAPIInitialized());
            ResetAPIState();
        }

        [NativeDllLoadedTrigger]
        [InitializeOnLoadMethod]
        public static void SetupAPI()
        {
            Debug.Log("Unity CPP API Setting Up... IsAPIInitialized() value: " + CppAPIMethods.IsAPIInitialized());
            if(CppAPIMethods.IsAPIInitialized() == 1) CppAPIMethods.DestroyAPI();
            CppAPIMethods.InitializeAPI();
            SetupLogHandlers();
            Debug.Log("Unity CPP API Setup, IsAPIInitialized() value: " + CppAPIMethods.IsAPIInitialized());
            
            Hardware.Setup();
        }

        private static void LogCPPBuffer(LogLevel logLevel)
        {
            int outputBufferSize = CppAPIMethods.GetOutputBufferSize();
            StringBuilder buffer = new StringBuilder(outputBufferSize);
            CppAPIMethods.ReadOutputBuffer(buffer);
            
            if (logLevel == LogLevel.Info || logLevel == LogLevel.Debug)
                Debug.Log(buffer);
            else if (logLevel == LogLevel.Warning)
                Debug.LogWarning(buffer);
            else if (logLevel == LogLevel.Error || logLevel == LogLevel.Exception)
                Debug.LogError(buffer);
        }

        private void Start()
        {
            Test();
        }
        
        public static void Test()
        {
            if (CppAPIMethods.IsAPIInitialized() == 1)
            {
                if (!robotInitialized)
                {
                    RobotEvents.RobotInitialize();
                    RobotEvents.CompetitionInitialize();
                    RobotEvents.InitializeAutonomous();
                    robotInitialized = true;
                }
                RobotEvents.UpdateAutonomous();
            }
        }

        //https://stackoverflow.com/questions/39790977/how-to-pass-a-delegate-or-function-pointer-from-c-sharp-to-c-and-call-it-there
        private static void SetupLogHandlers()
        {
            logInfoCallback = () => LogCPPBuffer(LogLevel.Info);
            logDebugCallback = () => LogCPPBuffer(LogLevel.Debug);
            logWarnCallback = () => LogCPPBuffer(LogLevel.Warning);
            logErrCallback = () => LogCPPBuffer(LogLevel.Error);
            logExceptCallback = () => LogCPPBuffer(LogLevel.Exception);

            CppAPIMethods.SetLogInfoListener(Marshal.GetFunctionPointerForDelegate(logInfoCallback));
            CppAPIMethods.SetLogDebugListener(Marshal.GetFunctionPointerForDelegate(logDebugCallback));
            CppAPIMethods.SetLogWarnListener(Marshal.GetFunctionPointerForDelegate(logWarnCallback));
            CppAPIMethods.SetLogErrListener(Marshal.GetFunctionPointerForDelegate(logErrCallback));
            CppAPIMethods.SetLogExceptListener(Marshal.GetFunctionPointerForDelegate(logExceptCallback));
        }

        public static void ResetAPIState()
        {
            // Loading/Unloading DLL Resets API's state
            DLLReloaderUnity.Unload();
            DLLReloaderUnity.Load();
        }

        public static void RunAPITests()
        {
            CppAPIMethods.RunAPITests();
        }
        
        public static void DestroyAPI()
        {
            CppAPIMethods.DestroyAPI();
        }
    }
}