using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityNativeTool;

namespace VexSimulator.SimulatorAPI.UnsafeCppAPI
{
    [MockNativeDeclarations]
    public static class UnsafeLogging
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void ReadOutputBuffer(StringBuilder outBuffer);

        [DllImport("CPPSimulatorAPI")]
        public static extern int GetOutputBufferSize();

        [DllImport("CPPSimulatorAPI")]
        public static extern void SetLogInfoListener(IntPtr aCallback);
        
        [DllImport("CPPSimulatorAPI")]
        public static extern void SetLogDebugListener(IntPtr aCallback);
        
        [DllImport("CPPSimulatorAPI")]
        public static extern void SetLogWarnListener(IntPtr aCallback);

        [DllImport("CPPSimulatorAPI")]
        public static extern void SetLogErrListener(IntPtr aCallback);

        [DllImport("CPPSimulatorAPI")]
        public static extern void SetLogExceptListener(IntPtr aCallback);
    }
}