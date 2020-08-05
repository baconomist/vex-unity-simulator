using System;
using VexSimulator.SimulatorAPI;

namespace VexSimulator
{
    // TODO: add Attribute [SafetyCheck] that runs the type's SafetyCheck() function
    // TODO: or maybe an [Unsafe] Attribute to warn the user of an unsafe method    

    /**
     * This attribute doesn't actually do anything, just tell's the programmer that this is a threaded method
     */
    public class ThreadedMethodAttribute : System.Attribute
    {
    }
    
    /**
     * Used on classes that share fields to the CPP API
     */
    [System.AttributeUsage(AttributeTargets.Class)]
    public class SharableAPIDeviceAttribute : System.Attribute
    {
        public readonly string deviceType;
        
        public SharableAPIDeviceAttribute(string deviceType)
        {
            this.deviceType = deviceType;
        }
    }

    /**
     * Used on fields that are shared to the CPP API
     */
    [System.AttributeUsage(AttributeTargets.Field)]
    public class SharableAPIParamFieldAttribute : System.Attribute
    {
        public readonly string paramName;
        public readonly bool isValueReceiver;

        public SharableAPIParamFieldAttribute(string overrideParamName = null, bool isValueReceiver = true)
        {
            paramName = overrideParamName;
            this.isValueReceiver = isValueReceiver;
        }
    }
    
    /**
     * Used on methods that are meant to receive param field values
     */
    [System.AttributeUsage(AttributeTargets.Method)]
    public class SharableAPIParamMethodAttribute : System.Attribute
    {
        public readonly string paramName;
        public readonly bool isValueReceiver;

        public SharableAPIParamMethodAttribute(string paramName, bool isValueReceiver)
        {
            this.paramName = paramName;
            this.isValueReceiver = isValueReceiver;
        }
    }
}