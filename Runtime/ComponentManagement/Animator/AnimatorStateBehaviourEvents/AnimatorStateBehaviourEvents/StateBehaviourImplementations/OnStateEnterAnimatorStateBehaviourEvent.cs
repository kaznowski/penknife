using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class OnStateEnterAnimatorStateBehaviourEvent : AnimatorManagementStateBehaviourEvent
    {
        /// <summary>
        /// OnStateMove is called right after Animator.OnAnimatorMove(). It normally implements code that processes and affects root motion
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateBehaviourTrigger(animator, stateInfo, layerIndex);
        }
    }
}