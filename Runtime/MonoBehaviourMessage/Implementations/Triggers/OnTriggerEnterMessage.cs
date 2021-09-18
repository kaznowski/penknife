using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerEnterMessage : MonoBehaviourMessage_T1<Collider>
    {
        private void OnTriggerEnter(Collider collider)
        {
            TriggerEventT1(collider);
        }
    }
}