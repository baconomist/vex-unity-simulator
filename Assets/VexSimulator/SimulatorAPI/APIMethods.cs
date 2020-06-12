using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityNativeTool;
using VexSimulator.SimulatorAPI.UnsafeCppAPI;

namespace VexSimulator.SimulatorAPI
{
    public class APIMethods : MonoBehaviour
    {
        public static void Reload()
        {
            if (UnsafeCppAPI.UnsafeAPIMethods.IsAPIInitialized() == 1) DestroyAPI();
            Debug.Log("Reloading Simulator API...");
            ResetState();
        }

        [NativeDllLoadedTrigger]
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            Debug.Log("Initializing Simulator API...");
            if (UnsafeCppAPI.UnsafeAPIMethods.IsAPIInitialized() == 1) UnsafeCppAPI.UnsafeAPIMethods.DestroyAPI();
            UnsafeCppAPI.UnsafeAPIMethods.InitializeAPI();
            Debug.Log("Initialized Simulator API...");

            Debug.Log("Setting up the rest of the API...");
            Logging.SetupLogHandlers();
            Hardware.Setup();
        }

        public static void Test()
        {
            RequireAPIInitialized();

            UnsafeCppAPI.UnsafeRobotEvents.RobotInitialize();
            UnsafeCppAPI.UnsafeRobotEvents.CompetitionInitialize();
            UnsafeCppAPI.UnsafeRobotEvents.InitializeAutonomous();

            new Thread(() => { UnsafeCppAPI.UnsafeRobotEvents.UpdateAutonomous(); }).Start();
        }

        public static void ResetState()
        {
            // Loading/Unloading DLL Resets the API's state as the DLL is the "instance" of the API
            DLLReloaderUnity.Unload();
            DLLReloaderUnity.Load();
        }

        public static void RunTests()
        {
            RequireAPIInitialized();

            UnsafeCppAPI.UnsafeAPIMethods.RunAPITests();
        }

        public static void DestroyAPI()
        {
            RequireAPIInitialized();

            UnsafeCppAPI.UnsafeAPIMethods.DestroyAPI();
        }

        public static void RequireAPIInitialized()
        {
            if (UnsafeCppAPI.UnsafeAPIMethods.IsAPIInitialized() != 1)
                throw new Exception("API initialization is required to call this method.");
        }
    }
}