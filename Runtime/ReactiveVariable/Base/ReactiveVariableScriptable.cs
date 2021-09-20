using System;
using UnityEngine;

namespace DoubleDash.CodingTools.ReactiveVariables
{
    //[CreateAssetMenu(fileName = "New Reactive <YourVariableHere>", menuName = "DoubleDash/CodingTools/ReactiveVariables/Variable/<YourVariableHere>")]
    public abstract class ReactiveVariableScriptable<TypeVariable> : ScriptableObject, IVariable<TypeVariable>,
        IReactiveVariable<TypeVariable>, IEventSubscriber<DelegateEventSubscriber<TypeVariable>>,
        ISerializationCallbackReceiver
    {
        [SerializeField]
        VariableReference<TypeVariable> _initialValue = new VariableReference<TypeVariable>();

        [SerializeField] ReactiveVariable<TypeVariable> _runtimeValue = new ReactiveVariable<TypeVariable>(default(TypeVariable));

        #region Properties

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnBeforeValueChangedEvent =>
            _runtimeValue.OnBeforeValueChangedEvent;

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnValueChangedEvent =>
            _runtimeValue.OnValueChangedEvent;

        public TypeVariable Value
        {
            get => _runtimeValue.Value;
            set => _runtimeValue.Value = value;
        }

        public IEventSlotHandle Subscribe(DelegateEventSubscriber<TypeVariable> subscriber)
        {
            return _runtimeValue.Subscribe(subscriber);
        }

        public void SetValue(TypeVariable value)
        {
            Value = value;
        }

        #endregion

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            
        }

        protected void OnEnable()
        {
            _runtimeValue?.SetValueAndForceNotify(_initialValue.Value);
        }

        protected void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            _runtimeValue?.Dispose();
        }
    }
}