namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class LateUpdateMessage : MonoBehaviourMessage
    {
        private void LateUpdate()
        {
            TriggerEvent();
        }
    }
}