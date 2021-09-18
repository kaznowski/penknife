using System;
using UnityEngine.UIElements;

public static class EventSlotExtensions
{
    #region IEventSubscriber withot T and Action without T
    public static IEventSlotHandle Subscribe(this IEventSubscriber<DelegateEventSubscriber> me, Action callback)
    {
        return me.Subscribe(new DelegateEventSubscriber(callback, () => true));
    }
    
    public static IEventSlotHandle SubscribeOnce(this IEventSubscriber<DelegateEventSubscriber> me, Action callback)
    {
        return me.SubscribeTimesX(callback, 1);
    }
    public static IEventSlotHandle SubscribeWhile(this IEventSubscriber<DelegateEventSubscriber> me, Action callback, Func<bool> whileFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber(callback, whileFunc));
    }
    
    public static IEventSlotHandle SubscribeUntil(this IEventSubscriber<DelegateEventSubscriber> me, Action callback, Func<bool> untilFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber(callback, () => !untilFunc()));
    }
    
    private static IEventSlotHandle SubscribeTimesX(this IEventSubscriber<DelegateEventSubscriber> me, Action callback, int xTimes)
    {
        return me.Subscribe(new DelegateEventSubscriber(callback, () => true, xTimes));
    }
    #endregion
    
    #region IEventSubscriber<T> with Action without T

    /// <summary>
    /// Register on a EventSlot with parameters with a callback that has no parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="me"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEventSlotHandle Subscribe<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action callback)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>((arg1) => callback(), () => true));
    }

    /// <summary>
    /// Register once on a EventSlot with parameters with a callback that has no parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="me"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEventSlotHandle SubscribeOnce<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action callback)
    {
        return me.SubscribeTimesX(callback, 1);
    }
    public static IEventSlotHandle SubscribeWhile<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action callback, Func<bool> whileFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>((arg1)=>callback(), whileFunc));
    }
    
    public static IEventSlotHandle SubscribeUntil<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action callback, Func<bool> untilFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>((arg1)=>callback(), () => !untilFunc()));
    }
    
    private static IEventSlotHandle SubscribeTimesX<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action callback, int xTimes)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>((arg1)=>callback(), () => true, xTimes));
    }
    

    #endregion

    #region IEventSubscriber<T> with Action<T>
    public static IEventSlotHandle Subscribe<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action<T> callback)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>(callback, () => true));
    }
    
    public static IEventSlotHandle SubscribeOnce<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action<T> callback)
    {
        return me.SubscribeTimesX(callback, 1);
    }
    
    public static IEventSlotHandle SubscribeWhile<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action<T> callback, Func<bool> whileFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>(callback, whileFunc));
    }
    
    public static IEventSlotHandle SubscribeUntil<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action<T> callback, Func<bool> untilFunc)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>(callback,  () => !untilFunc()));
    }
    
    private static IEventSlotHandle SubscribeTimesX<T>(this IEventSubscriber<DelegateEventSubscriber<T>> me, Action<T> callback, int xTimes)
    {
        return me.Subscribe(new DelegateEventSubscriber<T>(callback,  () => true, xTimes));
    }
    #endregion
}