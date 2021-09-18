using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerStay2DMessage : MonoBehaviourMessage_T1<Collider2D>
    {
        private void OnTriggerStay2D(Collider2D collider)
        {
            TriggerEventT1(collider);
        }
    }
}