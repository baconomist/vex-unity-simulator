using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.UnsafeCppAPI
{
    [MockNativeDeclarations]
    public static class UnsafeRobotEvents
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void Initialize();

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

        // [HandleProcessCorruptedStateExceptions]
        // [SecurityCritical]
        [DllImport("CPPSimulatorAPI")]
        public static extern void UpdateAutonomous();
    }
}