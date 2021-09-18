using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class GameEventBase<TypeUnityEvent, TypeEventSlot, TypeDelegate> : IGameEvent<TypeDelegate>
    where TypeUnityEvent : UnityEventBase
    where TypeEventSlot  : EventSlotBase<TypeDelegate>, new()
    where TypeDelegate   : DelegateEventSubscriberBase
{
    [SerializeField, Tooltip("Unity event used for injection of persistent event calls.")]
    protected TypeUnityEvent unityEvent;

    //Used for dynamic subscribing of events
    private TypeEventSlot eventController;

    protected TypeEventSlot Event
    {
        get
        {
            if (eventController == null)
                eventController = new TypeEventSlot();
            return eventController;
        }
    }

    public bool IsDisposed => Event.IsDisposed;

    #region DelegateEventSubscriber

    public IEventSlotHandle Subscribe(TypeDelegate subscriber)
    {
        return Event.Subscribe(subscriber);
    }
    #endregion

    public abstract void Trigger();

    public void Dispose()
    {
        if (eventController == null)
            return;
        Event.Dispose();
    }

    public void Refresh()
    {
        eventController = null;
    }
}

[System.Serializable]
public class GameEvent : GameEventBase<UnityEvent, EventSlot, DelegateEventSubscriber>
{
    /// <summary>
    /// Use DefaultValue for trigger (from Unity Editor Serialization).
    /// </summary>
    public override void Trigger()
    {
        if (IsDisposed) return;
        if (Event.HasListeners) Event.Trigger();
        try
        {
            if (unityEvent?.GetPersistentEventCount() > 0) unityEvent?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}

[System.Serializable]
public class GameEvent<TypeEventArgs> : GameEventBase<UnityEvent<TypeEventArgs>, EventSlot<TypeEventArgs>, DelegateEventSubscriber<TypeEventArgs>>
{
    [SerializeField, Tooltip("Arguments sent by default if the event does not offer any parameters.")]
    private TypeEventArgs _defaultValue;

    public TypeEventArgs DefaultValue => _defaultValue;

    #region Triggers

    public void Trigger(TypeEventArgs arg1)
    {
        if (IsDisposed) return;
        if (Event.HasListeners) Event.Trigger(arg1);
        try
        {
            if (unityEvent?.GetPersistentEventCount() > 0) unityEvent?.Invoke(arg1);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Use DefaultValue for trigger (from Unity Editor Serialization).
    /// </summary>
    public override void Trigger()
    {
        Trigger(DefaultValue);
    }

    /// <summary>
    /// Set DefaultValue for trigger. REMEMBER: it will override the Unity Editor value, but only in Playmode.
    /// This method exists so that it may be called through Unity's interface.
    /// </summary>
    public void SetDefaultValue(TypeEventArgs value)
    {
        _defaultValue = value;
    }

    #endregion
}