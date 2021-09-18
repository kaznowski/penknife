namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class StartMessage : MonoBehaviourMessage
    {
        private void Start()
        {
            TriggerEvent();
        }
    }
}