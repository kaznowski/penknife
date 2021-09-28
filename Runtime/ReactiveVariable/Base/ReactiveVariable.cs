using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.ReactiveVariables
{
    public interface IReactiveVariable<TypeVariable> : IDisposable
    {
        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnBeforeValueChangedEvent 
        {
            get;
        }

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnValueChangedEvent
        {
            get;
        }
    }

    [System.Serializable]
    public class ReactiveVariable<TypeVariable> : IVariable<TypeVariable>, IReactiveVariable<TypeVariable>, IEventSubscriber<DelegateEventSubscriber<TypeVariable>>
    {
        [SerializeProperty("Value")]
        [SerializeField] TypeVariable _variable;

        [Header("Events")]
        [SerializeField]
        MultiStepGameEvent<TypeVariable> _onBeforeValueChanged = new MultiStepGameEvent<TypeVariable>();

        [SerializeField]
        MultiStepGameEvent<TypeVariable> _onValueChanged = new MultiStepGameEvent<TypeVariable>();

        [Header("Settings")] [
            
        Tooltip("Should the event attached to this variable trigger even when the attributed value is the same as the previous value?")]
        public bool triggerRedundantAttributions;

        #region Properties

        public TypeVariable Value
        {
            get 
            {
                return _variable;
            }
            set 
            {
                //If both values are different...
                if (!EqualityComparer<TypeVariable>.Default.Equals(_variable, value) || triggerRedundantAttributions) 
                {
                    SetInternal(value);
                }
            }
        }    

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnValueChangedEvent => _onValueChanged;

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnBeforeValueChangedEvent => _onBeforeValueChanged;

        #endregion

        #region Public Methods

        public ReactiveVariable(TypeVariable initialValue)
        {
            _variable = initialValue;
        }
        
        public void SetValueAndForceNotify(TypeVariable newValue) 
        {
            SetInternal(newValue);
        }

        /// <summary>
        /// Set the variable's value to the parameter value. This function is useful for using with unity's graphical interface.
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(TypeVariable newValue) 
        {
            Value = newValue;    
        }

        #endregion

        #region Private Methods

        void SetInternal(TypeVariable newValue) 
        {
            //Call the event for the before value changes.
            OnBeforeValueChangedEvent?.Trigger(_variable);

            //Update the value
            _variable = newValue;

            //Call the event for the value change.
            OnValueChangedEvent?.Trigger(_variable);
        }

        #endregion

        public IEventSlotHandle Subscribe(DelegateEventSubscriber<TypeVariable> subscriber)
        {
            return _onValueChanged.Subscribe(subscriber);
        }

        public void Dispose()
        {
            _onBeforeValueChanged?.Dispose();
            _onValueChanged?.Dispose();
        }
    }
}