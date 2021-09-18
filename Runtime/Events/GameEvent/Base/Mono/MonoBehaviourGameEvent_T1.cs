using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleDash.CodingTools.GameEvents
{
    public abstract class MonoBehaviourGameEvent<TypeEventArgs> : MonoBehaviourGameEventBase<DelegateEventSubscriber<TypeEventArgs>>, 
        IGameEvent<DelegateEventSubscriber<TypeEventArgs>, TypeEventArgs>
    {
        [SerializeField]
        protected MultiStepGameEvent<TypeEventArgs> _event = new MultiStepGameEvent<TypeEventArgs>();

        public TypeEventArgs DefaultValue => _event.DefaultValue;

        public void SetDefaultValue(TypeEventArgs value)
        {
            _event.SetDefaultValue(value);
        }

        public void Trigger(TypeEventArgs arg1)
        {
            _event.Trigger(arg1);
        }

        public override void Trigger() {
            Trigger(DefaultValue);
        }
    }
}

