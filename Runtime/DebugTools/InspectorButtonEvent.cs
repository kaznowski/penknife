using UnityEngine;

namespace DoubleDash.CodingTools.DebugTools
{
    public class InspectorButtonEvent : MonoBehaviour, IEventSubscriber<DelegateEventSubscriber>
    {
        [TextArea]
        [Tooltip("What does this button do?")]
        public string description;

        [Tooltip("Events fired when the button is pressed.")]
        public GameEvent onPress;

        //[Sirenix.OdinInspector.Button("Button")]
        public void Button()
        {
            onPress.Trigger();
        }

        public IEventSlotHandle Subscribe(DelegateEventSubscriber subscriber)
        {
            return onPress.Subscribe(subscriber);
        }
    }
}
