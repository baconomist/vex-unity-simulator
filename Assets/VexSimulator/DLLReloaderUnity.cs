using System;
using UnityEditor;
using UnityEngine;

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

        public static DLLReloaderUnity Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DLLReloaderUnity>();
                return _instance;
            }
        }

        private void OnValidate()
        {
            EditorApplication.update -= UpdateReloader;
            EditorApplication.update += UpdateReloader;
        }

        private static void UpdateReloader()
        {
            if (Instance.wasActive != UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                if (!UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                {
                    UnityNativeTool.Internal.DllManipulator.UnloadAll();
                    EditorApplication.isPaused = false;
                }
                else
                {
                    UnityNativeTool.Internal.DllManipulator.LoadAll();
                    EditorApplication.isPaused = false;
                }
            }

            Instance.wasActive = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
        }
    }
}