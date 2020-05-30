using System.Runtime.InteropServices;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.CppAPI
{
    [MockNativeDeclarations]
    public class RobotEvents
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void RobotInitialize();

        [DllImport("CPPSimulatorAPI")]
        public static extern void CompetitionInitialize();

        [DllImport("CPPSimulatorAPI")]
        public static extern void CompetitionDisable();

        [DllImport("CPPSimulatorAPI")]
        public static extern void InitializeOpControl();

        [DllImport("CPPSimulatorAPI")]
        public static extern void InitializeAutonomous();

        [DllImport("CPPSimulatorAPI")]
        public static extern void UpdateOpControl();

        [DllImport("CPPSimulatorAPI")]
        public static extern void UpdateAutonomous();
    }
}