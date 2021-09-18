using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerExit2DMessage : MonoBehaviourMessage_T1<Collider2D>
    {
        private void OnTriggerExit2D(Collider2D collider)
        {
            TriggerEventT1(collider);
        }
    }
}