namespace DoubleDash.MonoBehaviourMessages
{
    public class OnEnableMessage : MonoBehaviourMessage
    {
        private void OnEnable()
        {
            TriggerEvent();
        }
    }
}