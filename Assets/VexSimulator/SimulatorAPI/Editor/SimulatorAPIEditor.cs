using System.Threading;
using UnityEditor;
using UnityEngine;
using VexSimulator.SimulatorAPI;
using VexSimulator.SimulatorAPI.UnsafeCppAPI;

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
                SimulatorAPI.APIMethods.TestAuton();

            if (GUILayout.Button("Test Something"))
            {
                Debug.Log("ParamVal " + ParamHandler.RequestParam(1, "Motor", "voltage").int_val);
                // Debug.Log(ParamHandler.RequestParam(11, "Vision", "rgbColor").int_val);
                AttributeResolver.ResolveAttributes();
                ParamHandler.UpdateRegisteredParams();
            }

            base.OnInspectorGUI();
        }
    }
}