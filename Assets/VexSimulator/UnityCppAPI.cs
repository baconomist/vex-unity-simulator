using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityNativeTool;

namespace VexSimulator
{
    public class UnityCppAPI : MonoBehaviour
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void VoidCallback();
        
        private enum LogLevel
        {
            Info,
            Debug,
            Warning,
            Error,
            Exception
        }

        private static VoidCallback logInfoCallback;
        private static VoidCallback logDebugCallback;
        private static VoidCallback logWarnCallback;
        private static VoidCallback logErrCallback;
        private static VoidCallback logExceptCallback;

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
                Debug.Log("API Init True");
                CppAPIMethods.UpdateOpControl();
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