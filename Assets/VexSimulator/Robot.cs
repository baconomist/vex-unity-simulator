using UnityEngine;
using VexSimulator.SimulatorAPI.CppAPI;

namespace VexSimulator
{
    public class Robot : MonoBehaviour
    {
        private bool _robotInitialized = false;
        private bool _autonomousInitialized = false;
        private bool _opControlInitialized = false;
        
        public void InitializeRobot()
        {
            RobotEvents.RobotInitialize();
            RobotEvents.CompetitionInitialize();
        }

        public void Autonomous()
        {
            if (!_autonomousInitialized)
            {
                RobotEvents.InitializeAutonomous();
                _autonomousInitialized = true;
            }

            RobotEvents.UpdateAutonomous();
        }
        
        public void OpControl()
        {
            if (!_opControlInitialized)
            {
                RobotEvents.InitializeOpControl();
                _opControlInitialized = true;
            }

            RobotEvents.UpdateOpControl();
        }
    }
}