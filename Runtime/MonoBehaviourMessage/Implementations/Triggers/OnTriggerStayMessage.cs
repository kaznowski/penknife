using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerStayMessage : MonoBehaviourMessage_T1<Collider>
    {
        private void OnTriggerStay(Collider collider)
        {
            TriggerEventT1(collider);
        }
    }
}