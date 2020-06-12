using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityNativeTool;

// https://www.codeproject.com/Articles/12673/Calling-Managed-NET-C-COM-Objects-from-Unmanaged-C

namespace VexSimulator.SimulatorAPI.UnsafeCppAPI
{
    [MockNativeDeclarations]
    // ReSharper disable once InconsistentNaming
    public static class UnsafeAPIMethods
    {
        [DllImport("CPPSimulatorAPI")]
        public static extern void InitializeAPI();
        
        [DllImport("CPPSimulatorAPI")]
        public static extern void DestroyAPI();

        [DllImport("CPPSimulatorAPI")]
        public static extern int IsAPIInitialized();
        
        [DllImport("CPPSimulatorAPI")]
        public static extern int RunAPITests();
    }
}