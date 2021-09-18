using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionExitMessage : MonoBehaviourMessage_T1<Collision>
    {
        private void OnCollisionExit(Collision collision)
        {
            TriggerEventT1(collision);
        }
    }
}