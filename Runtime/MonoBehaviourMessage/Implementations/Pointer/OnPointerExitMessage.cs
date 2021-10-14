using UnityEngine.EventSystems;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnPointerExitMessage : MonoBehaviourMessage_T1<PointerEventData>,
        IPointerExitHandler
    {
        public void OnPointerExit(PointerEventData eventData)
        {
            TriggerEventT1(eventData);
        }
    }
}
