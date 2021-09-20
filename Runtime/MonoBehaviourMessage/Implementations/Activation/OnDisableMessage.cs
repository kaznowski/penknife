namespace DoubleDash.MonoBehaviourMessages
{
    public class OnDisableMessage : MonoBehaviourMessage
    {
        private void OnDisable()
        {
            TriggerEvent();
        }
    }
}