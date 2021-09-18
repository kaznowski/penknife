namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class OnDisableMessage : MonoBehaviourMessage
    {
        private void OnDisable()
        {
            TriggerEvent();
        }
    }
}