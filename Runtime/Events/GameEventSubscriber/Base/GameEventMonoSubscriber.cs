using UnityEngine;
using DoubleDash.CodingTools;

public abstract class GameEventMonoSubscriber<TypeVariable> : MonoBehaviour
{
    [Tooltip("IEventSubscriber that 'onEventTriggered' will subscribe to.")]
    public VariableReference<IEventSubscriber<DelegateEventSubscriber<TypeVariable>>> gameEvent;

    [Tooltip("These callbacks will be triggered when the reactive variable changes.")]
    //public VariableReference<IGameEvent<DelegateEventSubscriber<TypeVariable>, TypeVariable>> onEventTriggered;
    public GameEvent<TypeVariable> onEventTriggered = new GameEvent<TypeVariable>();

    [Header("Settings")]
    [Tooltip("Mode by which 'onEventTriggered' will be subscribed to 'gameEvent'.")]
    public EventSubscriptionMode subscriptionMode = EventSubscriptionMode.always;
    [Tooltip("If the mode above is WhileCondition or UntilCondition, the 'onEventTriggered' will be unsubscribed when the condition becomes false or true (respectivelly).")]
    public VariableReference<bool> whileCondition = new VariableReference<bool>(true);

    public enum EventSubscriptionMode
    {
        always,
        once,
        whileCondition,
        untilCondition
    }

    //Handles for unsubscribing
    IEventSlotHandle eventHandle;

    #region Public Functions

    /// <summary>
    /// Called when the reactive variable this is attached to changes its value.
    /// </summary>
    /// <param name="variable"></param>
    public void OnEvent(TypeVariable variable)
    {
        onEventTriggered.Trigger(variable);
    }

    #endregion

    #region Private Functions

    IEventSlotHandle SubscribeToEvent()
    {
        switch (subscriptionMode)
        {
            case (EventSubscriptionMode.always):
                return gameEvent.Value.Subscribe(OnEvent);
            case (EventSubscriptionMode.once):
                return gameEvent.Value.SubscribeOnce(OnEvent);
            case (EventSubscriptionMode.whileCondition):
                return gameEvent.Value.SubscribeWhile(OnEvent, () => whileCondition.Value);
            case (EventSubscriptionMode.untilCondition):
                return gameEvent.Value.SubscribeUntil(OnEvent, () => whileCondition.Value);
        }

        return null;
    }

    #endregion

    #region MonoBehaviour Messages

    private void OnEnable()
    {
        eventHandle = SubscribeToEvent();
    }

    private void OnDisable()
    {
        eventHandle.Dispose();
    }

    #endregion
}