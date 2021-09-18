using System;
using UnityEngine;

namespace DoubleDash.CodingTools.GameEvents
{
    public abstract class MonoBehaviourGameEventBase<TypeDelegateEventSubscriber> : MonoBehaviour, 
       IGameEvent<TypeDelegateEventSubscriber>, 
       IVariable<IGameEvent<TypeDelegateEventSubscriber>>
       where TypeDelegateEventSubscriber : DelegateEventSubscriberBase
    {
        public abstract IGameEvent<TypeDelegateEventSubscriber> Value
        {
            get;
            set;
        }

        public bool IsDisposed => throw new NotImplementedException();

        [InspectorButton("Trigger Event")]
        public abstract void Trigger();

        [InspectorButton("Dispose of Event")]
        public void Dispose()
        {
            Value.Dispose();
        }

        [InspectorButton("Refresh Event")]
        public void Refresh()
        {
            Value.Refresh();
        }

        public void OnEnable()
        {
            Refresh();
        }

        public void OnDisable()
        {
            Dispose();
        }

        public IEventSlotHandle Subscribe(TypeDelegateEventSubscriber subscriber)
        {
            return Value.Subscribe(subscriber);
        }
    }
}

