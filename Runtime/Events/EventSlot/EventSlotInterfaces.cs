using System;

#region Subscribers

public interface IEventSubscriber<TypeDelegateEventSubscriber>
    where TypeDelegateEventSubscriber : DelegateEventSubscriberBase
{
    IEventSlotHandle Subscribe(TypeDelegateEventSubscriber subscriber);
}

#endregion

#region Triggers

/// <summary>
/// Used for when the event must be triggered by passing a specific parameter on that Invoke.
/// </summary>
/// <typeparam name="TypeEventArgs"></typeparam> 
public interface IEventTrigger<TypeEventArgs>
{
    void Trigger(TypeEventArgs arg1);
}

/// <summary>
/// Trigger event with generic parameter.
/// </summary>
public interface IEventTrigger
{
    void Trigger();
}

#endregion

#region IDisposable extensions

public interface ICancelable : IDisposable
{
    bool IsDisposed { get; }
}

/// <summary>
/// Reference to the registered event's callback. This is used to unregister a callback from an event, without having to know from which event are you unsubscribing. And without having to know the registered action.
/// </summary>
public interface IEventSlotHandle : ICancelable
{
    
}

#endregion

/// <summary>
/// Equivalent to a null event that prevents null propagation. This is used for events that have already been disposed.
/// </summary>
public class EmptyEventSlotHandle : IEventSlotHandle
{
    private static EmptyEventSlotHandle _eventSlotHandle;

    public bool IsDisposed => true;

    public static EmptyEventSlotHandle Empty
    {
        get
        {
            if (_eventSlotHandle == null)
                _eventSlotHandle = new EmptyEventSlotHandle();
            return _eventSlotHandle;
        }
    }

    /// <summary>
    /// This private constructor limits the creation of empty eventslothandles 
    /// </summary>
    private EmptyEventSlotHandle()
    {
       
    }

    public void Dispose()
    {
        
    }
}