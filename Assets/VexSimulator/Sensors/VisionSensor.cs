using System;
using UnityEngine;
using VexSimulator.SimulatorAPI;

namespace VexSimulator.Sensors
{
    [SharableAPIDevice(DeviceTypes.Vision)]
    public class VisionSensor : SharableAPIDevice
    {
        private Light _light;
        private Color _currentColor;

        [HideInInspector]
        [SharableAPIParamField()]
        public int rgbColor;
        
        private void Start()
        {
            _light = GetComponentInChildren<Light>();
        }

        private void Update()
        {
            // Can only be set on main thread
            _light.color = _currentColor;
        }

        [ThreadedMethod]
        private void OnVisionLEDChange(int visionPort, int rgb)
        {
            int r = (rgb >> 16) & 0xff;
            int g = (rgb >> 8) & 0xff;
            int b = rgb & 0xff;

            _currentColor = new Color(NormalizeHexColorChannel(r), NormalizeHexColorChannel(g),
                NormalizeHexColorChannel(b));
        }

        /**
         * Returns a range 0-1 for Color() struct use
         **/
        private static float NormalizeHexColorChannel(int channel)
        {
            return Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 16 * 16, channel));
        }

        [SharableAPIParamMethod("rgbColor", true)]
        public void OnLedChange(int value)
        {
            
        }
    }
}