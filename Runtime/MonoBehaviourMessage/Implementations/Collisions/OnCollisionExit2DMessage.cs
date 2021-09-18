using UnityEngine;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnCollisionExit2DMessage : MonoBehaviourMessage_T1<Collision2D>
    {
        private void OnCollisionExit2D(Collision2D collision)
        {
            TriggerEventT1(collision);
        }
    }
}