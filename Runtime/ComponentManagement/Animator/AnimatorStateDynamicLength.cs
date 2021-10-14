using DoubleDash.CodingTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class AnimatorStateDynamicLength : MonoBehaviour
    {
        public VariableReference<float> seconds = new VariableReference<float>(1f);

        public string multiplierParameter = "speedMultiplier";

        public void SetAnimationStateTime(AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters)
        {
            float multiplierValue;

            //Solve instant caseses that would result in division by zero
            if (seconds.Value == 0)
            {
                multiplierValue = float.MaxValue;
            }
            else
            {
                //Avoid cases where the multiplier is affecting the length
                float speedWithoutMultiplier = parameters.animatorStateInfo.length * parameters.animatorStateInfo.speedMultiplier;

                //If the original animation is meant to be instant
                if (speedWithoutMultiplier == 0)
                {
                    multiplierValue = float.MaxValue;
                }
                else
                {
                    Debug.Log(parameters.animatorStateInfo.shortNameHash + " " + speedWithoutMultiplier);

                    float speedToNormalizeAnimation = 1 / speedWithoutMultiplier;
                    float speedToSetAnimationLength = speedToNormalizeAnimation / seconds.Value;

                    multiplierValue = speedToSetAnimationLength;
                }
            }

            //Set
            parameters.animator.SetFloat(multiplierParameter, multiplierValue);
        }

        public void ResetAnimationStateTime(AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters)
        {
            //Set
            parameters.animator.SetFloat(multiplierParameter, 1);
        }
    }
}
