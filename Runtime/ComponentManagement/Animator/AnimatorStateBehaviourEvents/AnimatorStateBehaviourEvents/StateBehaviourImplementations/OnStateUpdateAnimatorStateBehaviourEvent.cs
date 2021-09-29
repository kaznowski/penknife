using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class OnStateUpdateAnimatorStateBehaviourEvent : AnimatorManagementStateBehaviourEvent
    {
        /// <summary>
        /// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateBehaviourTrigger(animator, stateInfo, layerIndex);
        }
    }
}
