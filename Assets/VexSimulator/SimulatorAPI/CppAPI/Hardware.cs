using System;
using System.Runtime.InteropServices;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.CppAPI
{
    [MockNativeDeclarations]
    public class Hardware
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern int GetMotorVoltage(int motorPort);
        
        [DllImport("CPPSimulatorAPI")]
        public static extern void RegisterMotorVoltageChangeCallback(IntPtr callback, int motor_port);
    }
}