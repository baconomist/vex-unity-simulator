using System;
using UnityEngine;

namespace VexSimulator
{
    [RequireComponent(typeof(WheelCollider))]
    public class MotorizedWheel : MonoBehaviour
    {
        public int port;
        public int voltage;

        private WheelCollider _wheelCollider;

        public void SetMotorVoltage(int motorVoltage)
        {
            voltage = motorVoltage;
        }

        private void Start()
        {
            _wheelCollider = GetComponent<WheelCollider>();
        }

        private void Update()
        {
            _wheelCollider.motorTorque = voltage;
        }
    }
}