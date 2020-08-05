using System;
using System.Runtime.InteropServices;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.UnsafeCppAPI
{
    [MockNativeDeclarations]
    public static class UnsafeParamHandler
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void SetParamRequestListener(IntPtr paramRequestListener);

        [DllImport("CPPSimulatorAPI")]
        public static extern void RequestParam(ref ParamRequestResponse paramRequestResponse);

        [DllImport("CPPSimulatorAPI")]
        public static extern ParamRequestResponse CreateParamRequestResponse(int port, string deviceType,
            string paramName, string msg,
            int intVal = 0, float floatVal = 0);
    }
}