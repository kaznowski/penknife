using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionEnterMessage : MonoBehaviourMessage_T1<Collision>
    {
        private void OnCollisionEnter(Collision collision)
        {
            TriggerEventT1(collision);
        }
    }
}