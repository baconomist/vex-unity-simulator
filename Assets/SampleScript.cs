using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

// https://www.codeproject.com/Articles/12673/Calling-Managed-NET-C-COM-Objects-from-Unmanaged-C

public class SampleScript : MonoBehaviour
{
    public bool validate;
    
    // https://www.youtube.com/watch?v=w3jGgTHJoCY
    [DllImport("CPPSimulatorAPI")]
    static extern IntPtr GetSharedRobotAPI();

    [DllImport("CPPSimulatorAPI")]
    static extern void DeleteSharedRobotAPI(IntPtr api);

    [DllImport("CPPSimulatorAPI")]
    static extern float GetControlVelocity(IntPtr api);

    [DllImport("CPPSimulatorAPI")]
    static extern float NotifyOfVelocity(IntPtr api, float velocity);
    
    private IntPtr _sharedApi;

    public string GetHelloWorld()
    {
        return "Hello World!";
    }

    void OnValidate ()
    {
        _sharedApi = GetSharedRobotAPI();
        NotifyOfVelocity(_sharedApi, 1000f);
        Debug.Log(GetControlVelocity(_sharedApi));
        NotifyOfVelocity(_sharedApi, 20f);
        Debug.Log(GetControlVelocity(_sharedApi));
    }

    private void OnDestroy()
    {
        DeleteSharedRobotAPI(_sharedApi);
    }

    // https://stackoverflow.com/questions/778590/calling-c-sharp-code-from-c
    // https://github.com/3F/DllExport
//    [DllExport]
//    public static string TestCSharp()
//    {
//        return "It works!";
//    }
}