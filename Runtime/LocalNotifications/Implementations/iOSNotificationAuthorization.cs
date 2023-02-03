#if UNITY_IOS
using System.Collections;
using Unity.Notifications.iOS;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc/>
    public class iOSNotificationAuthorization : INotificationAuthorization
    {
        private const AuthorizationOption AuthorizationOption = Unity.Notifications.iOS.AuthorizationOption.Alert |
                                                                Unity.Notifications.iOS.AuthorizationOption.Badge |
                                                                Unity.Notifications.iOS.AuthorizationOption.CarPlay |
                                                                Unity.Notifications.iOS.AuthorizationOption.Sound;

        /// <inheritdoc/>
        public IEnumerator RequestAuthorization()
        {
            using var req = new AuthorizationRequest(AuthorizationOption, true);
            while (!req.IsFinished)
            {
                yield return null;
            }

            var res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;

            Debug.Log(res);
        }
    }
}
#endif
