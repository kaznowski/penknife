using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class OnStateIKAnimatorStateBehaviourEvent : AnimatorManagementStateBehaviourEvent
    {
        /// <summary>
        /// OnStateIK is called right after Animator.OnAnimatorIK(). It normally implements code that sets up animation IK (inverse kinematics)
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateBehaviourTrigger(animator, stateInfo, layerIndex);
        }
    }
}