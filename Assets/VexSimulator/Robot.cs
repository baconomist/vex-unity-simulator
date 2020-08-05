using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VexSimulator.SimulatorAPI;

namespace VexSimulator
{
    public class Robot : MonoBehaviour
    {
        public enum RobotMode
        {
            Autonomous,
            OpControl
        }

        public RobotMode mode;
        
        public bool robotInitialized = false;
        public bool competitionInitialized = false;
        public bool autonomousInitialized = false;
        public bool opControlInitialized = false;

        private Thread _robotThread;
        private bool _robotThreadRunning = false;
        private bool _canRobotThreadUpdate = false;

        private MotorizedWheel[] _wheels;
        
        public void Awake()
        {
            Simulator.RobotInitialize -= InitializeRobot;
            Simulator.RobotInitialize += InitializeRobot;

            Simulator.CompetitionInitialize -= InitializeCompetition;
            Simulator.CompetitionInitialize += InitializeCompetition;

            Simulator.RobotDisable -= DisableRobot;
            Simulator.RobotDisable += DisableRobot;

            Simulator.Autonomous -= RunAutonomous;
            Simulator.Autonomous += RunAutonomous;

            Simulator.OpControl -= RunOpControl;
            Simulator.OpControl += RunOpControl;
        }

        public void Start()
        {
            _wheels = GetComponentsInChildren<MotorizedWheel>();
        }

        private void RunAutonomous()
        {
            mode = RobotMode.Autonomous;
            // Sync robot thread w/ Update thread via this variable
            _canRobotThreadUpdate = true;
        }

        private void RunOpControl()
        {
            mode = RobotMode.OpControl;
            // Sync robot thread w/ Update thread via this variable
            _canRobotThreadUpdate = true;
        }

        private void InitializeRobot()
        {
            if (!robotInitialized)
            {
                RobotEvents.RobotInitialize();
                robotInitialized = true;
            }

            // Start after robot initialized
            CreateRobotThread();

            robotInitialized = true;
        }

        private void DisableRobot()
        {
            RobotEvents.CompetitionDisable();
        }

        private void InitializeCompetition()
        {
            RobotEvents.CompetitionInitialize();
            competitionInitialized = true;
        }

        [ThreadedMethod]
        private void Autonomous()
        {
            if (!autonomousInitialized)
            {
                RobotEvents.InitializeAutonomous();
                autonomousInitialized = true;
            }

            RobotEvents.UpdateAutonomous();
        }

        [ThreadedMethod]
        private void OpControl()
        {
            if (!opControlInitialized)
            {
                RobotEvents.InitializeOpControl();
                opControlInitialized = true;
            }

            RobotEvents.UpdateOpControl();
        }

        [ThreadedMethod]
        private void RobotThread()
        {
            while (_robotThreadRunning)
            {
                // Wait for update availability
                if (!_canRobotThreadUpdate) continue;

                if (mode == RobotMode.Autonomous)
                    Autonomous();
                else if (mode == RobotMode.OpControl)
                    OpControl();

                _canRobotThreadUpdate = false;
            }
        }

        [ThreadedMethod]
        private void OnMotorVoltageChange(int motorPort, int motorVoltage)
        {
            foreach (MotorizedWheel wheel in _wheels)
            {
                if(wheel.port == motorPort)
                    wheel.SetMotorVoltage(motorVoltage);
            }
        }

        private void CreateRobotThread()
        {
            _robotThreadRunning = true;
            _robotThread = new Thread(RobotThread);
            _robotThread.Start();
        }

        // Called before OnDestroy()
        private void OnApplicationQuit()
        {
            if (_robotThread != null || _robotThreadRunning)
            {
                _robotThreadRunning = false;
                // Join the thread so we can wait for it to stop
                _robotThread.Join();
            }
        }
    }
}