using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [System.Serializable]
    public class AnimatorManagerParameter
    {
        [Tooltip("What is the name of the parameter in the animator?")]
        public string parameterName = "new Parameter";

        [Tooltip("What is the type of this parameter?" + "\n\n" +
                 "A parameter of the type 'Trigger' does not require a value to be set - It simply activates the trigger with that 'Parameter Name'")]
        public AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Trigger;

        public int   intValue   = 0;
        public float floatValue = 0f;
        public bool  boolValue  = false;
    }
}