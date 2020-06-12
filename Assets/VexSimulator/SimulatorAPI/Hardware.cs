using System.Runtime.InteropServices;
using UnityEngine;

namespace VexSimulator.SimulatorAPI
{
    public static class Hardware
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MotorVoltageChangeCallback(int motorPort, int motorVoltage);

        public static event MotorVoltageChangeCallback OnMotorVoltageChange;

        public static void Setup()
        {
            SetupMotorCallbacks();
        }

        [ThreadedMethod]
        private static void OnMotorVoltageChangeListener(int motorPort, int motorVoltage)
        {
            OnMotorVoltageChange?.Invoke(motorPort, motorVoltage);
        }

        private static void SetupMotorCallbacks()
        {
            UnsafeCppAPI.UnsafeHardware.SetMotorVoltageChangeCallback(Marshal.GetFunctionPointerForDelegate((MotorVoltageChangeCallback) OnMotorVoltageChangeListener));
        }
    }
}