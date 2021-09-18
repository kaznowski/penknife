using UnityEngine;

namespace DoubleDash.CodingTools.GameEvents
{
    [CreateAssetMenu(fileName = "NewVoidGameEvent", menuName = "DoubleDash/GameEvents/Void GameEvent")]
    public class ScriptableGameEvent : ScriptableGameEventBase<DelegateEventSubscriber>
    {
        [SerializeField]
        MultiStepGameEvent _event = new MultiStepGameEvent();

        public override IGameEvent<DelegateEventSubscriber> Value { get => _event; set => _event = (MultiStepGameEvent)value; } //This "set" may return null if the interface isnt a multistep game event.

        public override void Trigger()
        {
            _event.Trigger();
        }
    }
}

