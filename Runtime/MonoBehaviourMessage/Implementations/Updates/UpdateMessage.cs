namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class UpdateMessage : MonoBehaviourMessage
    {
        private void Update()
        {
            TriggerEvent();
        }
    }
}