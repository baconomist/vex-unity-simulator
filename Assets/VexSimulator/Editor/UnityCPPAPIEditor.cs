using UnityEditor;
using UnityEngine;

namespace VexSimulator.Editor
{
    [CustomEditor(typeof(UnityCppAPI))]
    public class UnityCPPAPIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Reset/Reload API"))
                UnityCppAPI.ReloadAPI();
            
            if(GUILayout.Button("Run API Tests"))
                UnityCppAPI.RunAPITests();
            
            if(GUILayout.Button("Run Test() Method"))
                UnityCppAPI.Test();

            base.OnInspectorGUI();
        }
    }
}