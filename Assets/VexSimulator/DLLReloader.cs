//using System;
//using System.Runtime.InteropServices;
//using UnityEngine;
//
//[ExecuteInEditMode]
//public class DLLReloader : MonoBehaviour
//{
//    [DllImport("DLLReloader.dll")]
//    static extern int TestDLLReloader();
//
//    [DllImport("DLLReloader.dll")]
//    static extern IntPtr CreateAPI();
//
//    [DllImport("DLLReloader.dll")]
//    static extern void SetDoneTxtPath(IntPtr api, string path);
//
//    [DllImport("DLLReloader.dll")]
//    static extern void SetDLLPath(IntPtr api, string path);
//
//    [DllImport("DLLReloader.dll")]
//    static extern void SetCopyPath(IntPtr api, string path);
//
//    [DllImport("DLLReloader.dll")]
//    static extern void SetOnUnloadListener(IntPtr api, IntPtr callback);
//
//    [DllImport("DLLReloader.dll")]
//    static extern void SetOnLoadListener(IntPtr api, IntPtr callback);
//
//    [DllImport("DLLReloader.dll")]
//    static extern void UpdateDLLReloader(IntPtr api);
//
//    public bool validate;
//
//    private bool _dllReloaderSetup = false;
//
//    public bool apiCreated = false;
//    private IntPtr _apiInstance;
//
//    private void OnDrawGizmos()
//    {
//#if UNITY_EDITOR
//        // Ensure continuous Update calls.
//        if (!Application.isPlaying)
//        {
//            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
//            UnityEditor.SceneView.RepaintAll();
//        }
//#endif
//    }
//
//    private void OnValidate()
//    {
//        Debug.Log(TestDLLReloader());
//
//        if (!apiCreated)
//        {
//            Debug.Log("Api Created!");
//            _apiInstance = CreateAPI();
//        }
//
//        apiCreated = true;
//        
//        SetDoneTxtPath(_apiInstance,
//            "C:\\Users\\Lucas\\Desktop\\Projects\\2020-4659-Vex\\CodeRepo\\RobotCode\\cmake-build-simulatorvscompile\\simulator_api\\Debug\\DONE.txt");
//        SetDLLPath(_apiInstance,
//            "C:\\Users\\Lucas\\Desktop\\Projects\\2020-4659-Vex\\CodeRepo\\RobotCode\\cmake-build-simulatorvscompile\\simulator_api\\Debug\\__CPPSimuatorAPI.dll");
//        SetCopyPath(_apiInstance,
//            "C:\\Users\\Lucas\\Desktop\\Projects\\2020-4659-Vex\\CodeRepo\\RobotUnityProject\\Assets\\Plugins\\__CPPSimuatorAPI.dll");
//
//        testCallbacks();
//
//        _dllReloaderSetup = true;
//    }
//
//    private void Update()
//    {
//        if (_dllReloaderSetup)
//            UpdateDLLReloader(_apiInstance);
//    }
//
//    public delegate void CallbackDelegate();
//
//    private CallbackDelegate d;
//
//    public void testCallbacks()
//    {
//        d = callback01;
//        SetOnLoadListener(_apiInstance, Marshal.GetFunctionPointerForDelegate(d));
//        SetOnUnloadListener(_apiInstance, Marshal.GetFunctionPointerForDelegate(d));
//    }
//
//    public void callback01()
//    {
//        Debug.Log("callback 01 called. Message");
//    }
//}