using UnityEngine;
using UnityEngine.Events;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Component used to serialize the call made when the user opens the app by tapping a notification.
    /// The data stored in the tapped notification is delivered as a parameter to the String Game Event.
    /// </summary>
    public class LastRespondedNotification : MonoBehaviour
    {
        [SerializeField] private UnityEvent<string> _onRetrievedLastRespondedNotificationGameEvent;

        private void Awake()
        {
            if (!NotificationPlatformsManager.TryCreateNotificationPlatform(out var platform))
                return;

            if (!platform.TryGetLastRespondedNotificationData(out var notification))
                return;

            _onRetrievedLastRespondedNotificationGameEvent.Invoke(notification.Data);
        }
    }
}
