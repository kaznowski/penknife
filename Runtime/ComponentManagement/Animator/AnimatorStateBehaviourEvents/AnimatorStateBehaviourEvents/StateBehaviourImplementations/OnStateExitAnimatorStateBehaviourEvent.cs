using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement 
{
    public class OnStateExitAnimatorStateBehaviourEvent : AnimatorManagementStateBehaviourEvent
    {
        /// <summary>
        /// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateBehaviourTrigger(animator, stateInfo, layerIndex);
        }
    }
}