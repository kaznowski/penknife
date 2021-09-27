using System;
using UnityEngine;

namespace DoubleDash.CodingTools.ReactiveVariables
{
    //[CreateAssetMenu(fileName = "New Reactive <YourVariableHere>", menuName = "DoubleDash/CodingTools/ReactiveVariables/Variable/<YourVariableHere>")]
    public abstract class ReactiveVariableScriptable<TypeVariable> : ScriptableObject, IVariable<TypeVariable>,
        IReactiveVariable<TypeVariable>, IEventSubscriber<DelegateEventSubscriber<TypeVariable>>
    {
        [SerializeField]
        [Tooltip("Since this is a ScriptableObject, whenever the editor exits playmode, the Runtime Value is reset to this Initial Value.")]
        VariableReference<TypeVariable> _initialValue = new VariableReference<TypeVariable>();

        [Tooltip("Since this is a ScriptableObject, whenever the editor enters playmode, this Runtime Value is reset to the Initial Value.")]
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

        protected virtual void OnEnable()
        {
            _runtimeValue?.SetValueAndForceNotify(_initialValue.Value);
        }

        protected virtual void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            _runtimeValue?.Dispose();
        }
    }
}