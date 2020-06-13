using System;
using System.Runtime.InteropServices;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.UnsafeCppAPI
{
    [MockNativeDeclarations]
    public static class UnsafeHardware
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void SetMotorVoltageChangeCallback(IntPtr callback);

        [DllImport("CPPSimulatorAPI")]
        public static extern void SetVisionLEDChangeCallback(IntPtr callback);
    }
}