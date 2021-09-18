namespace DoubleDash.MonoBehaviourMessages
{
    public class AwakeMessage : MonoBehaviourMessage
    {
        private void Awake()
        {
            TriggerEvent();
        }
    }
}