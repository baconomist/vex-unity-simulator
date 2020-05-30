using System;
using UnityEditor;
using UnityEngine;
using VexSimulator.SimulatorAPI;

/**
 * Unloads all loaded DLLs when you exit Unity and loads them back in when you enter.
 * This way you can make a symlink to your build dll and build it without file perm errors.
 * */
namespace VexSimulator
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UnityNativeTool.DllManipulatorScript))]
    public class DLLReloaderUnity : MonoBehaviour
    {
        [HideInInspector] public bool wasActive = false;
        
        private static DLLReloaderUnity _instance;

        // Ensure DLLs are unloaded when Unity starts
        [InitializeOnLoadMethod]
        private static void OnUnityLoad()
        {
            Unload();
        }

        private void OnValidate()
        {
            _instance = this;
            EditorApplication.update -= UpdateReloader;
            EditorApplication.update += UpdateReloader;
        }

        // When we focus/unfocus Unity, load/unload the DLLs
        private static void UpdateReloader()
        {
            if (_instance.wasActive != UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                if (!UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                {
                    Unload();
                }
                else
                {
                    Load();
                }
            }

            _instance.wasActive = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
        }
        
        public static void Load()
        {
            UnityNativeTool.Internal.DllManipulator.LoadAll();
            EditorApplication.isPaused = false;
        }

        public static void Unload()
        {
            // Need to destroy the api on unload so that the VectoredExceptionHandler is removed, otherwise a crash occurs
            // It's probably good to do this here anyways
            UnityCppAPI.DestroyAPI();
            UnityNativeTool.Internal.DllManipulator.UnloadAll();
            EditorApplication.isPaused = false;
        }
    }
}