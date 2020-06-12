using System;
using UnityEngine;
using VexSimulator.SimulatorAPI.UnsafeCppAPI;

namespace VexSimulator
{
    public class Simulator : MonoBehaviour
    {
        public enum SimulationMode
        {
            Competition,
            Autonomous,
            OpControl,
            TestingManual
        }

        public SimulationMode mode;
        
        public SimulatorAPI.Logging.LogLevel loggingLevel = SimulatorAPI.Logging.LogLevel.None;

        public static event Action RobotInitialize;
        public static event Action CompetitionInitialize;
        public static event Action RobotDisable;
        public static event Action Autonomous;
        public static event Action OpControl;

        private void OnValidate()
        {
            SimulatorAPI.Logging.loggingLevel = loggingLevel;
        }

        private void Start()
        {
            RobotInitialize?.Invoke();
            // TODO: find a place for this
            CompetitionInitialize?.Invoke();
        }

        private void Update()
        {
            if (mode == SimulationMode.Autonomous)
            {
                Autonomous?.Invoke();
            }
            else if (mode == SimulationMode.OpControl)
            {
                OpControl?.Invoke();
            }
        }
    }
}