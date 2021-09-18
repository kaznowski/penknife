using UnityEngine;

namespace DoubleDash.CodingTools.ReactiveVariables
{
    //[CreateAssetMenu(fileName = "New Reactive <YourVariableHere>", menuName = "DoubleDash/CodingTools/ReactiveVariables/Variable/<YourVariableHere>")]
    public abstract class ReactiveVariableScriptable<TypeVariable> : ScriptableObject, IVariable<TypeVariable>, IReactiveVariable<TypeVariable>, IEventSubscriber<DelegateEventSubscriber<TypeVariable>>
    {
        [SerializeField]
        ReactiveVariable<TypeVariable> _value;

        #region Properties

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnBeforeValueChangedEvent => _value.OnBeforeValueChangedEvent;

        public IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable> OnValueChangedEvent => _value.OnValueChangedEvent;

        public TypeVariable Value { get => _value.Value; set => _value.Value = value; }

        public IEventSlotHandle Subscribe(DelegateEventSubscriber<TypeVariable> subscriber)
        {
            return _value.Subscribe(subscriber);
        }

        #endregion
    }
}
