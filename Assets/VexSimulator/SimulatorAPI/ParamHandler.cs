using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Web;
using UnityEngine;
using UnityEngine.Networking;
using VexSimulator.Sensors;
using VexSimulator.SimulatorAPI.UnsafeCppAPI;

namespace VexSimulator.SimulatorAPI
{
    public static class DeviceTypes
    {
        public const string Motor = "Motor";
        public const string Vision = "Vision";
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ParamRequestResponse
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string paramUri;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string msg;

        public int int_val;
        public float float_val;
    };

    public struct SharedParamInfo
    {
        public string paramName;
        public bool isReceiver;
        public Type objectType;
        public FieldInfo fieldInfo;
        public bool isMethod;
        public MethodInfo methodInfo;
    }

    // TODO: add Setup() safety check
    public static class ParamHandler
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ParamRequestResponse ParamRequestListener(ParamRequestResponse paramRequestResponse);

        private static List<SharableAPIDevice> _sharableApiDevices =
            new List<SharableAPIDevice>();

        private static List<SharedParamInfo> _sharedParamInfos = new List<SharedParamInfo>();

        public static void Setup()
        {
            _sharableApiDevices = new List<SharableAPIDevice>();
            _sharedParamInfos = new List<SharedParamInfo>();

            UnsafeCppAPI.UnsafeParamHandler.SetParamRequestListener(
                Marshal.GetFunctionPointerForDelegate((ParamRequestListener) OnParamRequestReceived));

            AttributeResolver.ResolveAttributes();
        }

        public static void UpdateRegisteredParams()
        {
            foreach (SharedParamInfo sharedParamInfo in _sharedParamInfos)
            {
                if (sharedParamInfo.isReceiver)
                {
                    foreach (SharableAPIDevice device in _sharableApiDevices)
                    {
                        if (device.GetType() == sharedParamInfo.objectType)
                        {
                            ParamRequestResponse paramRequestResponse = UnsafeParamHandler.CreateParamRequestResponse(
                                device.port,
                                device.GetType().GetCustomAttribute<SharableAPIDeviceAttribute>().deviceType,
                                sharedParamInfo.paramName, "");
                            UnsafeParamHandler.RequestParam(ref paramRequestResponse);

                            if (sharedParamInfo.isMethod && sharedParamInfo.isReceiver)
                            {
                                // Update required methods
                                foreach (MethodInfo methodInfo in device.GetType().GetMethods())
                                {
                                    if (sharedParamInfo.methodInfo.Name == methodInfo.Name)
                                    {
                                        if (methodInfo.GetParameters()[0].ParameterType == typeof(int))
                                            methodInfo.Invoke(device, new object[] {paramRequestResponse.int_val});
                                        else if (methodInfo.GetParameters()[0].ParameterType == typeof(float))
                                            methodInfo.Invoke(device, new object[] {paramRequestResponse.float_val});

                                        // Once we've updated the method we can exit the loop
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // Update required fields
                                foreach (FieldInfo fieldInfo in device.GetType().GetRuntimeFields())
                                {
                                    if (sharedParamInfo.fieldInfo.Name == fieldInfo.Name)
                                    {
                                        if (fieldInfo.FieldType == typeof(int))
                                            fieldInfo.SetValue(device, paramRequestResponse.int_val);
                                        else if (fieldInfo.FieldType == typeof(float))
                                            fieldInfo.SetValue(device, paramRequestResponse.float_val);

                                        // Once we've updated the field we can exit the loop
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static ParamRequestResponse OnParamRequestReceived(ParamRequestResponse paramRequestResponse)
        {
            Debug.Log(paramRequestResponse.paramUri);

            ParsedParamRequest parsedParamRequest = new ParsedParamRequest(paramRequestResponse);

            foreach (SharedParamInfo sharedParamInfo in _sharedParamInfos)
            {
                // If method and if the method "transmits" a value rather than receives it
                if (sharedParamInfo.isMethod && !sharedParamInfo.isReceiver)
                {
                    foreach (SharableAPIDevice device in _sharableApiDevices)
                    {
                        if (device.GetType() == sharedParamInfo.objectType)
                        {
                            foreach (MethodInfo methodInfo in device.GetType().GetMethods())
                            {
                                // Get value to return from a given method
                                if (sharedParamInfo.methodInfo.Name == methodInfo.Name)
                                {
                                    if (methodInfo.ReturnType == typeof(int))
                                        paramRequestResponse.int_val = (int) methodInfo.Invoke(device, null);
                                    else if (methodInfo.ReturnType == typeof(float))
                                        paramRequestResponse.float_val = (float) methodInfo.Invoke(device, null);
                                    
                                    // Once we've retrieved the param we can exit the loop
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return paramRequestResponse;
        }

        public static ParamRequestResponse RequestParam(int devicePort, string deviceType, string paramName)
        {
            ParamRequestResponse paramRequestResponse =
                UnsafeParamHandler.CreateParamRequestResponse(devicePort, deviceType, paramName, "Testing");
            UnsafeParamHandler.RequestParam(ref paramRequestResponse);
            return paramRequestResponse;
        }

        public static void RegisterSharedFieldParam(Type objectType, FieldInfo field)
        {
            string pName = field.GetCustomAttribute<SharableAPIParamFieldAttribute>().paramName;
            if (pName == null)
                pName = field.Name;

            _sharedParamInfos.Add(new SharedParamInfo
            {
                paramName = pName,
                isReceiver = field.GetCustomAttribute<SharableAPIParamFieldAttribute>().isValueReceiver,
                objectType = objectType,
                fieldInfo = field,
                isMethod = false,
                methodInfo = null
            });
        }

        public static void RegisterSharedFieldParam(Type objectType, MethodInfo method)
        {
            _sharedParamInfos.Add(new SharedParamInfo
            {
                paramName = method.GetCustomAttribute<SharableAPIParamMethodAttribute>().paramName,
                isReceiver = method.GetCustomAttribute<SharableAPIParamMethodAttribute>().isValueReceiver,
                objectType = objectType,
                fieldInfo = null,
                isMethod = true,
                methodInfo = method
            });
        }

        public static void RegisterDeviceInstance(SharableAPIDevice sharableApiDevice)
        {
            if (!_sharableApiDevices.Contains(sharableApiDevice))
                _sharableApiDevices.Add(sharableApiDevice);
            else
            {
                Debug.LogWarning(
                    $"Attempted to register already existing device! Typeof: <{sharableApiDevice.GetType()}>");
            }
        }
    }

    public class ParsedParamRequest
    {
        public ParamRequestResponse paramRequestResponse;
        public int port;
        public string paramName;
        public string deviceType;

        public ParsedParamRequest(ParamRequestResponse paramRequestResponse)
        {
            this.paramRequestResponse = paramRequestResponse;
            string adjustedParamUri = paramRequestResponse.paramUri.Replace("/<Brain>/", "/");

            Uri myUri = new Uri("https:/" + adjustedParamUri);

            port = int.Parse(adjustedParamUri.Split('/')[0]);
            paramName = HttpUtility.ParseQueryString(myUri.Query).Get("paramName");
            deviceType = HttpUtility.ParseQueryString(myUri.Query).Get("deviceType");
        }
    }
}