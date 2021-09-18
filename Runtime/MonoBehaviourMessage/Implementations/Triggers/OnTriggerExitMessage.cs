using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerExitMessage : MonoBehaviourMessage_T1<Collider>
    {
        private void OnTriggerExit(Collider collider)
        {
            TriggerEventT1(collider);
        }
    }
}