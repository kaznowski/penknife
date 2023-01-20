using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    public class ClearDisplayedNotifications : MonoBehaviour
    {
        [SerializeField] private bool _runOnStart = true;

        private void Start()
        {
            if (!_runOnStart)
                return;

            ClearAllDisplayedNotifications();
        }

        public void ClearAllDisplayedNotifications()
        {
            if (!NotificationPlatformsManager.TryCreateNotificationPlatform(out var platform))
                return;

            platform.ClearAllDisplayedNotifications();
        }
    }
}
