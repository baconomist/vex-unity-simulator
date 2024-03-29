using System;
using System.Runtime.ExceptionServices;
using System.Security;
using UnityEngine;

namespace VexSimulator.SimulatorAPI
{
    // TODO: add safety check
    public static class RobotEvents
    {
        private static bool _robotInitialized = false;
        private static bool _autonomousInitialized = false;
        private static bool _opControlInitialized = false;
        
        public static void RobotInitialize()
        {
            APIMethods.RequireAPIInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.Initialize();
            _robotInitialized = true;
        }

        public static void CompetitionInitialize()
        {
            APIMethods.RequireAPIInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.CompetitionInitialize();
        }

        public static void CompetitionDisable()
        {
            APIMethods.RequireAPIInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.CompetitionDisable();
        }

        public static void InitializeOpControl()
        {
            APIMethods.RequireAPIInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.InitializeOpControl();
            _opControlInitialized = true;
        }

        public static void InitializeAutonomous()
        {
            APIMethods.RequireAPIInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.Initialize();
            _autonomousInitialized = true;
        }

        public static void UpdateOpControl()
        {
            APIMethods.RequireAPIInitialized();
            RequireOpControlInitialized();
            UnsafeCppAPI.UnsafeRobotEvents.UpdateOpControl();
        }
        
        public static void UpdateAutonomous()
        {
            APIMethods.RequireAPIInitialized();
            RequireAutonomousInitialized();
            try
            {
                UnsafeCppAPI.UnsafeRobotEvents.UpdateAutonomous();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /**
         * Safety checks
         */
        public static void RequireRobotInitialized()
        {
            if(!_robotInitialized)
                throw new Exception("Cannot call method without calling RobotInitialize()");
        }
        
        public static void RequireAutonomousInitialized()
        {
            if(!_autonomousInitialized)
                throw new Exception("Cannot call method without calling InitializeAutonomous()");
        }
        
        public static void RequireOpControlInitialized()
        {
            if(!_opControlInitialized)
                throw new Exception("Cannot call method without calling InitializeOpControl()");
        }
    }
}