using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnTriggerEnter2DMessage : MonoBehaviourMessage_T1<Collider2D>
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            TriggerEventT1(collider);
        }
    }
}