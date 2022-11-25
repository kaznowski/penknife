using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for instantiating the object that handles the notifications for the current platform.
    /// </summary>
    public class NotificationPlatformsManager
    {
        /// <summary>
        /// Instantiates the object that handles the notifications for the current platform.
        /// </summary>
        /// <param name="notificationPlatform">The object that handles the notifications for the current platform.</param>
        /// <returns>True if instantiated successfully. False otherwise.</returns>
        public static bool TryCreateNotificationPlatform(out INotificationPlatform notificationPlatform)
        {
            #if UNITY_ANDROID
            notificationPlatform = new AndroidNotificationPlatform();
            return true;
            #endif

            #if UNITY_IOS
            notificationPlatform = new iOSNotificationPlatform();
            return true;
            #endif

            Debug.LogError("Could not create a Notifications Platform.");
            notificationPlatform = null;
            return false;
        }
    }
}
