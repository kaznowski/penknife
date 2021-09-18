namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class FixedUpdateMessage : MonoBehaviourMessage
    {
        private void FixedUpdate()
        {
            TriggerEvent();
        }
    }
}