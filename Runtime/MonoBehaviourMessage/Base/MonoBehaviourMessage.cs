using UnityEngine;
using UnityEngine.Events;

namespace DoubleDash.MonoBehaviourMessages
{
    public abstract class MonoBehaviourMessageBase<TypeGameEvent, TypeUnityEvent, TypeEventSlot, TypeDelegate> : MonoBehaviour, IEventSubscriber<TypeDelegate>
        where TypeGameEvent : GameEventBase<TypeUnityEvent, TypeEventSlot, TypeDelegate>
        where TypeUnityEvent : UnityEventBase
        where TypeEventSlot : EventSlotBase<TypeDelegate>, new()
        where TypeDelegate : DelegateEventSubscriberBase
    {
        [Tooltip("Events that are triggered when the associated MonoBehaviour function is called.")]
        public TypeGameEvent onMessageTrigger;

        public IEventSlotHandle Subscribe(TypeDelegate subscriber)
        {
            return onMessageTrigger.Subscribe(subscriber);
        }

        protected virtual void TriggerEvent()
        {
            onMessageTrigger.Trigger();
        }
    }

    public abstract class MonoBehaviourMessage : MonoBehaviourMessageBase<GameEvent, UnityEvent, EventSlot, DelegateEventSubscriber>
    {
    }

    public abstract class MonoBehaviourMessage_T1<TypeParameter> : MonoBehaviourMessageBase<GameEvent<TypeParameter>, UnityEvent<TypeParameter>, EventSlot<TypeParameter>, DelegateEventSubscriber<TypeParameter>>
    {
        protected virtual void TriggerEventT1(TypeParameter arg1)
        {
            onMessageTrigger.Trigger(arg1);
        }
    }
}