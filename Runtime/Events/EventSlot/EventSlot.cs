using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventSlotBase<TypeDelegateEventSubscriber> : IEventSubscriber<TypeDelegateEventSubscriber>,
    IEventSlotHandle
    where TypeDelegateEventSubscriber : DelegateEventSubscriberBase
{
    //Object to be used for controling multithreading semaphore
    protected readonly object lockHelper = new object();

    //List of all dynamic delegate events
    protected List<TypeDelegateEventSubscriber> slots = new List<TypeDelegateEventSubscriber>();

    //Tag to store whether this event/channel is valid. A disposed event is no longer valid.
    private bool _disposed;

    public bool IsDisposed => _disposed;
    public bool HasListeners => slots.Count != 0;

    #region IEventRegister implementation

    /// <summary>
    /// Subscribe to the event while obtaining the slot handle.
    /// </summary>
    /// <param name="subscriber"></param>
    /// <returns></returns>
    public IEventSlotHandle Subscribe(TypeDelegateEventSubscriber subscriber)
    {
        if (IsDisposed)
            return EmptyEventSlotHandle.Empty;
        slots.Add(subscriber);
        return subscriber;
    }
    #endregion

    public virtual void Dispose()
    {
        if (_disposed)
            return;

        foreach (var slot in slots)
        {
            slot.Dispose();
        }

        slots.Clear();
        _disposed = true;
    }

    protected abstract void TriggerEventSlot(TypeDelegateEventSubscriber listener);

    public void TriggerOnThread()
    {
        try
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                var listener = slots[i];

                //Remove listeners of disposed events that are still on the list
                if (listener.IsDisposed)
                {
                    slots.Remove(listener);
                }
                else
                {
                    try
                    {
                        TriggerEventSlot(listener);
                        if (listener.IsDisposed)
                        {
                            slots.Remove(listener);
                        }
                    }
                    catch (Exception e
                    ) //Catching exceptions prevent cases where, if an event fails, the remaining events break;
                    {
                        Debug.LogException(e);
                        slots.Remove(listener);
                    }
                }
            }
        }

        //Catch errors problems with problems with the event slot.
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

public class EventSlot : EventSlotBase<DelegateEventSubscriber>, IEventTrigger
{
    public void Trigger()
    {
        if (IsDisposed)
            return;

        //Protects this EventSlot from sinchronicity problems generated by multithreading.
        lock (lockHelper)
        {
            TriggerOnThread();
        }
    }

    protected override void TriggerEventSlot(DelegateEventSubscriber listener)
    {
        listener.Invoke();
    }
}

public class EventSlot<TypeEventArgs> : EventSlotBase<DelegateEventSubscriber<TypeEventArgs>>,
    IEventTrigger<TypeEventArgs>
{
    //Internal args
    TypeEventArgs currentArgs;

    public void Trigger(TypeEventArgs arg1)
    {
        if (IsDisposed)
            return;

        //Protects this EventSlot from sinchronicity problems generated by multithreading.
        lock (lockHelper)
        {
            //Set the argument
            currentArgs = arg1;
            TriggerOnThread();
        }
    }

    protected override void TriggerEventSlot(DelegateEventSubscriber<TypeEventArgs> listener)
    {
        listener.Invoke(currentArgs);
    }
}