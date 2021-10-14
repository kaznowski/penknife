using UnityEngine.EventSystems;

namespace DoubleDash.MonoBehaviourMessages
{
    public class OnPointerEnterMessage : MonoBehaviourMessage_T1<PointerEventData>,
        IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            TriggerEventT1(eventData);
        }
    }
}
