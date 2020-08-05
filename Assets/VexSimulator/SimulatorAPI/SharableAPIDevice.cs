using System;
using UnityEngine;

namespace VexSimulator.SimulatorAPI
{
    public class SharableAPIDevice : MonoBehaviour
    {
        public int port;
        
        private void OnValidate()
        {
            ParamHandler.RegisterDeviceInstance(this);
        }
    }
}