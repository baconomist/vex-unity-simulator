using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VexSimulator.Sensors;
using VexSimulator.SimulatorAPI;

namespace VexSimulator
{
    public static class AttributeResolver
    {
        public const string DLL_PATH_PATTERN_DLL_NAME_MACRO = "{name}";
        public const string DLL_PATH_PATTERN_ASSETS_MACRO = "{assets}";
        public const string DLL_PATH_PATTERN_PROJECT_MACRO = "{proj}";
        private const string CRASH_FILE_NAME_PREFIX = "unityNativeCrash_";

        public static readonly string[] DEFAULT_ASSEMBLY_NAMES =
        {
            "Assembly-CSharp"
#if UNITY_EDITOR
            , "Assembly-CSharp-Editor"
#endif
        };

        public static readonly string[] INTERNAL_ASSEMBLY_NAMES =
        {
            "mcpiroman.UnityNativeTool"
#if UNITY_EDITOR
            , "mcpiroman.UnityNativeTool.Editor"
#endif
        };

        public static readonly string[] IGNORED_ASSEMBLY_PREFIXES =
            {"UnityEngine.", "UnityEditor.", "Unity.", "com.unity.", "Mono.", "nunit."};

        public static void ResolveAttributes()
        {
            IEnumerable<string> assemblyPathsTemp = new List<string>();
            if (!assemblyPathsTemp.Any())
                assemblyPathsTemp = DEFAULT_ASSEMBLY_NAMES;

            assemblyPathsTemp = assemblyPathsTemp.Concat(INTERNAL_ASSEMBLY_NAMES);

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = allAssemblies.Where(a =>
                    !a.IsDynamic && assemblyPathsTemp.Any(p => p == Path.GetFileNameWithoutExtension(a.Location)))
                .ToArray();

            foreach (var assembly in assemblies)
            {
                var allTypes = assembly.GetTypes();
                foreach (var type in allTypes)
                {
                    foreach (var field in type.GetRuntimeFields())
                    {
                        if (field.GetCustomAttribute<SharableAPIParamFieldAttribute>() != null)
                        {
                            if (type.GetCustomAttribute<SharableAPIDeviceAttribute>() == null)
                                throw new Exception(
                                    $"Must declare class <{type}> with attribute [SharableAPIDevice] to use [SharableAPIParamField].");
                            if (type.BaseType != typeof(SharableAPIDevice))
                                throw new Exception($"Must class <{type}> must derive from SharableAPIDevice.");

                            ParamHandler.RegisterSharedFieldParam(type, field);
                        }
                    }

                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute<SharableAPIParamMethodAttribute>() != null)
                        {
                            if (method.GetCustomAttribute<SharableAPIParamMethodAttribute>().isValueReceiver)
                            {
                                if (method.GetParameters().Length != 1 ||
                                    (method.GetParameters()[0].ParameterType != typeof(int) &&
                                     method.GetParameters()[0].ParameterType != typeof(float)))
                                    throw new Exception(
                                        $"Method signature of receiver {type}.{method.Name}(); must be void {method.Name}(<int,float> paramValue);");
                            }
                            else
                            {
                                if (method.ReturnType != typeof(int) && method.ReturnType != typeof(float))
                                    throw new Exception(
                                        $"Method signature of writer {type}.{method.Name}(); must be <int,float>{method.Name}();");
                            }
                            
                            ParamHandler.RegisterSharedFieldParam(type, method);
                        }
                    }
                }
            }
        }
    }
}