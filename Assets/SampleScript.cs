using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;

// https://www.codeproject.com/Articles/12673/Calling-Managed-NET-C-COM-Objects-from-Unmanaged-C

public class SampleScript : MonoBehaviour
{
    public bool validate;

//    // https://www.youtube.com/watch?v=w3jGgTHJoCY
//    [DllImport("CPPSimulatorAPI")]
//    static extern IntPtr GetSharedRobotAPI();

//    [DllImport("CPPSimulatorAPI")]
//    static extern void DeleteSharedRobotAPI(IntPtr api);
//

    // https://stackoverflow.com/questions/58427937/c-sharp-capture-output-from-c-dll
    [DllImport("CPPSimulatorAPI")]
    static extern void UpdateAutonomous();

    [DllImport("CPPSimulatorAPI")]
    static extern int Test();

    [DllImport("CPPSimulatorAPI")]
    static extern void ReadOutputBuffer(StringBuilder outBuffer);

    [DllImport("CPPSimulatorAPI")]
    static extern int GetOutputBufferSize();

    private IntPtr _sharedApi;

    void OnValidate()
    {
//        _sharedApi = GetSharedRobotAPI();
        Debug.Log("Test Method: " + Test());
        UpdateAutonomous();
        UpdateAutonomous();
        UpdateAutonomous();

        int outputBufferSize = GetOutputBufferSize();

        StringBuilder buffer = new StringBuilder(outputBufferSize);
        ReadOutputBuffer(buffer);
        Debug.Log(buffer);
        
//        unsafe
//        {
//            string buffer = "";
//            int* arr = (int*) ReadOutputBuffer().ToPointer();
//            for (int i = 0; i < GetOutputBufferSize(); i++)
//            {
//                buffer += arr[i].ToString();
//            }
//
//            Debug.Log("Output Buffer: " + buffer);
//        }
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
//        DeleteSharedRobotAPI(_sharedApi);
    }
}