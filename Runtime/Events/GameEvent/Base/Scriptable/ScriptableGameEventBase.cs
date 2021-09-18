using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleDash.CodingTools.GameEvents
{
    public abstract class ScriptableGameEventBase<TypeDelegateEventSubscriber> : ScriptableObject, 
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
            //Debug.Log("Enable");
            Refresh();
        }

        public void OnDisable()
        {
            //Debug.Log("Disable");
            Dispose();
        }

        public IEventSlotHandle Subscribe(TypeDelegateEventSubscriber subscriber)
        {
            return Value.Subscribe(subscriber);
        }
    }
}

