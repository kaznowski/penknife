using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionEnter2DMessage : MonoBehaviourMessage_T1<Collision2D>
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            TriggerEventT1(collision);
        }
    }
}