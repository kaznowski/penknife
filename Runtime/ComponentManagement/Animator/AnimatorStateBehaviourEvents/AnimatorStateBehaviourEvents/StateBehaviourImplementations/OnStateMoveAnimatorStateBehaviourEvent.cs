using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class OnStateMoveAnimatorStateBehaviourEvent : AnimatorManagementStateBehaviourEvent
    {
        /// <summary>
        /// OnStateMove is called right after Animator.OnAnimatorMove(). It normally implements code that processes and affects root motion
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateBehaviourTrigger(animator, stateInfo, layerIndex);
        }
    }
}
