using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityNativeTool;

// https://www.codeproject.com/Articles/12673/Calling-Managed-NET-C-COM-Objects-from-Unmanaged-C

namespace VexSimulator.SimulatorAPI.CppAPI
{
    [MockNativeDeclarations]
    // ReSharper disable once InconsistentNaming
    public class CppAPIMethods
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void InitializeAPI();
        
        [DllImport("CPPSimulatorAPI")]
        public static extern void DestroyAPI();

        [DllImport("CPPSimulatorAPI")]
        public static extern int IsAPIInitialized();
        
        [DllImport("CPPSimulatorAPI")]
        public static extern int RunAPITests();

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