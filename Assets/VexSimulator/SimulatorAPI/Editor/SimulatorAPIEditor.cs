using UnityEditor;
using UnityEngine;
using VexSimulator.SimulatorAPI;

namespace VexSimulator.Editor
{
    [CustomEditor(typeof(SimulatorAPI.SimulatorAPI))]
    public class SimulatorAPIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Reset/Reload API"))
                SimulatorAPI.SimulatorAPI.ReloadAPI();
            
            if(GUILayout.Button("Run API Tests"))
                SimulatorAPI.SimulatorAPI.RunAPITests();

            if(GUILayout.Button("Run Test() Method"))
                SimulatorAPI.SimulatorAPI.Test();

            base.OnInspectorGUI();
        }
    }
}