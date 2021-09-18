namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class OnEnableMessage : MonoBehaviourMessage
    {
        private void OnEnable()
        {
            TriggerEvent();
        }
    }
}