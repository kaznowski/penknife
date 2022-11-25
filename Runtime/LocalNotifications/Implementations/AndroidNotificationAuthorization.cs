#if UNITY_ANDROID
using System.Collections;
using UnityEngine.Android;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc/>
    public class AndroidNotificationAuthorization : INotificationAuthorization
    {
        private const string AndroidPermissionPostNotifications = "android.permission.POST_NOTIFICATIONS";

        /// <inheritdoc/>
        public IEnumerator RequestAuthorization()
        {
            if (!Permission.HasUserAuthorizedPermission(AndroidPermissionPostNotifications))
            {
                Permission.RequestUserPermission(AndroidPermissionPostNotifications);
            }

            yield return null;
        }
    }
}
#endif
