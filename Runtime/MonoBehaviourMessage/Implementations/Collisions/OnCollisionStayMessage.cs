using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionStayMessage : MonoBehaviourMessage_T1<Collision>
    {
        private void OnCollisionStay(Collision collision)
        {
            TriggerEventT1(collision);
        }
    }
}