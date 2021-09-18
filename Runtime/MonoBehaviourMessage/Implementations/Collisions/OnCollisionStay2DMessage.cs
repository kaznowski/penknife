using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionStay2DMessage : MonoBehaviourMessage_T1<Collision2D>
    {
        private void OnCollisionStay2D(Collision2D collision)
        {
            TriggerEventT1(collision);
        }
    }
}