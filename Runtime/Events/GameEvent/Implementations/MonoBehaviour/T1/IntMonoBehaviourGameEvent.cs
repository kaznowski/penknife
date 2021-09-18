namespace DoubleDash.CodingTools.GameEvents
{
    public class IntMonoBehaviourGameEvent : MonoBehaviourGameEvent<int>
    {
        public override IGameEvent<DelegateEventSubscriber<int>> Value { get => _event; set => _event = (MultiStepGameEvent<int>) value; }
    }
}