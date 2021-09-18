using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class MultiStepGameEventBase<TypeUnityEvent, TypeGameEvent, TypeEventSlot, TypeDelegate> : IGameEvent<TypeDelegate>
    where TypeUnityEvent : UnityEventBase
    where TypeGameEvent : GameEventBase<TypeUnityEvent, TypeEventSlot, TypeDelegate>, new()
    where TypeEventSlot : EventSlotBase<TypeDelegate>, new()
    where TypeDelegate : DelegateEventSubscriberBase
{

    //TEST OF EVENT LIST
    public List<EventListMember<TypeGameEvent>> events = new List<EventListMember<TypeGameEvent>>() {
        new EventListMember<TypeGameEvent>("Default"),
    };

    [System.Serializable]
    public class EventListMember<TypeEvent>
        where TypeEvent : new()
    {
        [SerializeField] string _name;
        [SerializeField] TypeEvent _event;

        public string Name => _name;
        public TypeEvent EventReference => _event;

        public EventListMember(string name) {
            this._name = name;
            _event = new TypeEvent();
        }
    }

    /// <summary>
    /// Gets an event with a given name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TypeGameEvent GetEvent(string name) 
    {
        foreach (EventListMember<TypeGameEvent> currentEvent in events)
        {
            if (currentEvent.Name == name) return currentEvent.EventReference;
        }
        return null;
    }

    /// <summary>
    /// Gets an event with a given name. If that event doesn't exist, create it.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TypeGameEvent GetOrCreateEvent(string name)
    {
        TypeGameEvent newEvent = GetEvent(name);

        if (newEvent == null) 
        {
            EventListMember<TypeGameEvent> newEventMember = new EventListMember<TypeGameEvent>(name);
            events.Add(newEventMember);
            return newEventMember.EventReference;
        }
        else 
        {
            return newEvent;
        }
    }

    public bool IsDisposed
    {
        get
        {
            //If a single event isn't disposed, then this isn't disposed
            foreach (EventListMember<TypeGameEvent> currentEvent in events)
                if (!currentEvent.EventReference.IsDisposed)
                    return false;

            //Every single event in this has been disposed of
            return true;
        }
    }

    //This is event is used for when this object is interacted with as a non multistage event 
    protected TypeGameEvent DefaultEvent 
    {
        get
        {
            //If there are no events on the list, generate a default event.
            if (events.Count <= 0) 
                events.Add(new EventListMember<TypeGameEvent>("Default"));

            //Return the default event
            return events[0].EventReference;
        }
    }

    public IEventSlotHandle Subscribe(TypeDelegate subscriber)
    {
        return DefaultEvent.Subscribe(subscriber);
    }

    [InspectorButton]
    public void Trigger()
    {
        foreach (EventListMember<TypeGameEvent> currentEvent in events)
            currentEvent.EventReference.Trigger();
    }

    [InspectorButton]
    public void Dispose()
    {
        foreach (EventListMember<TypeGameEvent> currentEvent in events)
            currentEvent.EventReference.Dispose();
    }

    [InspectorButton]
    public void Refresh()
    {
        foreach (EventListMember<TypeGameEvent> currentEvent in events)
            currentEvent.EventReference.Refresh();
    }
}

[System.Serializable]
public class MultiStepGameEvent : MultiStepGameEventBase<UnityEvent, GameEvent, EventSlot, DelegateEventSubscriber>
{

}

[System.Serializable]
public class MultiStepGameEvent<TypeEventArgs> : MultiStepGameEventBase<UnityEvent<TypeEventArgs>, 
    GameEvent<TypeEventArgs>,
    EventSlot<TypeEventArgs>,
    DelegateEventSubscriber<TypeEventArgs>>,
    IGameEvent<DelegateEventSubscriber<TypeEventArgs>, TypeEventArgs>
{
    [SerializeField, Tooltip("This default value will be used for all three game events.")] 
    TypeEventArgs _defaultValue;

    public TypeEventArgs DefaultValue => _defaultValue;

    /// <summary>
    /// This method exists so that it may be used by Unity's editor interface.
    /// </summary>
    /// <param name="value"></param>
    public void SetDefaultValue(TypeEventArgs value)
    {
        _defaultValue = value;
    }

    public void Trigger(TypeEventArgs arg1)
    {
        foreach (EventListMember<GameEvent<TypeEventArgs>> currentEvent in events)
            currentEvent.EventReference.Trigger(arg1);
    }
}
