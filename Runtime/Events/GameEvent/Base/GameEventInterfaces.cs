using UnityEngine;

public interface IRefreshable
{
    void Refresh();
}

/// <summary>
/// IGameEvent with ZERO parameters.
/// </summary>
/// <typeparam name="TypeDelegateSubscriber"></typeparam>
public interface IGameEvent<TypeDelegateSubscriber> : IEventSubscriber<TypeDelegateSubscriber>, IEventTrigger, IRefreshable, ICancelable, IEventSlotHandle
    where TypeDelegateSubscriber : DelegateEventSubscriberBase
{
}

/// <summary>
/// IGameEvent with ONE parameter.
/// </summary>
/// <typeparam name="TypeDelegateSubscriber"></typeparam>
/// <typeparam name="TypeEventArgs"></typeparam>
public interface IGameEvent<TypeDelegateSubscriber, TypeEventArgs> : IEventTrigger<TypeEventArgs>, IGameEvent<TypeDelegateSubscriber>
    where TypeDelegateSubscriber : DelegateEventSubscriberBase
{
    TypeEventArgs DefaultValue { get; }
    void SetDefaultValue(TypeEventArgs value);
}

/*
public interface IGameEventObject<TypeEventArgs> : IGameEvent<TypeEventArgs>
{
    IGameEvent<TypeEventArgs> OnPreEvent { get; }
    IGameEvent<TypeEventArgs> OnPosEvent { get; }
    IGameEvent<TypeEventArgs> OnEvent { get; }
    void OnEnable();
    void OnDisable();
}
*/
