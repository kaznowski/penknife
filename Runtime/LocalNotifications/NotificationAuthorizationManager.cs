using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Needs to be present at the first scene to guarantee the user has been requested to grant authorization to display notifications.
    /// </summary>
    public class NotificationAuthorizationManager : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_ANDROID && !UNITY_IOS
            return;
#endif


#if UNITY_ANDROID
            INotificationAuthorization notificationAuthorization;
            notificationAuthorization = new AndroidNotificationAuthorization();
            StartCoroutine(notificationAuthorization.RequestAuthorization());
#endif

#if UNITY_IOS
            INotificationAuthorization notificationAuthorization;
            notificationAuthorization = new iOSNotificationAuthorization();
            StartCoroutine(notificationAuthorization.RequestAuthorization());
#endif

        }
    }
}
