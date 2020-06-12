using UnityEditor;
using UnityEngine;
using VexSimulator.SimulatorAPI;

namespace VexSimulator.Editor
{
    [CustomEditor(typeof(SimulatorAPI.APIMethods))]
    public class SimulatorAPIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Reset/Reload API"))
                SimulatorAPI.APIMethods.Reload();
            
            if(GUILayout.Button("Run API Tests"))
                SimulatorAPI.APIMethods.RunTests();

            if(GUILayout.Button("Run Test() Method"))
                SimulatorAPI.APIMethods.Test();

            base.OnInspectorGUI();
        }
    }
}