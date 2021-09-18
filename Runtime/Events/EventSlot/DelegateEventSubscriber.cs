using System;

public abstract class DelegateEventSubscriberBase : IEventSlotHandle
{
    private bool _disposed = false;
    protected int _targetCount = int.MaxValue;
    protected int _triggeredCount = 0;
    protected Func<bool> _whileFunc;
    public bool IsDisposed => _disposed;
    public bool IsTriggerOnce => _targetCount == 1;

    public int TriggeredCount => _triggeredCount;

    protected abstract void
        DisposeOfAction(); //The appropriate action to dispose should be also determined in child classes

    public void Dispose()
    {
        DisposeOfAction();
        _disposed = true;
    }

    protected void AfterTrigger()
    {
        _triggeredCount++;
        //If the current eventCount it bigger then our targetCount, the event can be disposed
        if (_triggeredCount >= _targetCount)
            Dispose();
    }
}

public class DelegateEventSubscriber : DelegateEventSubscriberBase
{
    //Action with no parameters
    private Action callback;

    public DelegateEventSubscriber(Action callback, Func<bool> whileFunc, int targetCount = Int32.MaxValue)
    {
        this.callback = callback;
        this._targetCount = targetCount;
        _whileFunc = whileFunc;
    }

    protected override void DisposeOfAction()
    {
        callback = null;
    }

    public void Invoke()
    {
        //If this action delegate has been already disposed, do nothing.
        if (IsDisposed)
            return;
        //Check if While Func still true. If not, dispose the event.
        if (!_whileFunc())
        {
            Dispose();
            return;
        }

        //Invoke the appropriate action for this delegate
        if (callback != null)
        {
            callback.Invoke();
        }
        else
        {
            Dispose();
            return;
        }

        //Resolve additional effects after invoking the action
        AfterTrigger();
    }
}

/// <summary>
/// Class that stores the event's callback and if that event has been disposed.
/// </summary>
/// <typeparam name="TypeArgEvent"></typeparam>
public class DelegateEventSubscriber<TypeArgEvent> : DelegateEventSubscriberBase
{
    //Action with single parameter
    private Action<TypeArgEvent> callback;

    public DelegateEventSubscriber(Action<TypeArgEvent> callback, Func<bool> whileFunc,
        int targetCount = Int32.MaxValue)
    {
        this.callback = callback;
        this._targetCount = targetCount;
        _whileFunc = whileFunc;
    }

    protected override void DisposeOfAction()
    {
        callback = null;
    }

    /// <summary>
    /// Invokes an event passing an ar
    /// </summary>
    /// <param name="arg1"></param>
    public void Invoke(TypeArgEvent arg1)
    {
        //If this action delegate has been already disposed, do nothing.
        if (IsDisposed)
            return;

        //Check if While Func still true. If not, dispose the event.
        if (!_whileFunc())
        {
            Dispose();
            return;
        }

        //Invoke the appropriate action for this delegate
        if (callback != null)
        {
            callback.Invoke(arg1);
        }
        else
        {
            Dispose();
            return;
        }

        //Resolve additional effects after invoking the action
        AfterTrigger();
    }
}