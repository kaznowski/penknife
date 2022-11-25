using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for saving scheduled notifications, as well as automatically clearing expired notifications
    /// and canceling or rescheduling pending notifications when the device is restarted. 
    /// </summary>
    public class PendingNotificationsManager : MonoBehaviour
    {
        private static PendingNotificationsManager _instance;

        private List<PendingNotification> _pendingNotifications;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Deserialize();
                OnDeviceRestarted();
            }
            else
            {
                Serialize();
            }
        }

        /// <summary>
        /// Saves a scheduled notification, allowing it to be automatically cleared, canceled or rescheduled.
        /// </summary>
        /// <param name="notification">The object representing the notification in the current platform.</param>
        /// <param name="notificationDefinitionName">The name of the corresponding Notification Definition.</param>
        /// <param name="rescheduleOnDeviceRestart">Whether the notification should be rescheduled when the device restarts and it's still pending.</param>
        /// <param name="cancelOnDeviceRestart">Whether the notification should be canceled when the device restarts and it's still pending.</param>
        /// <param name="timeTrigger">How many minutes in the future the notification has been scheduled for.</param>
        /// <param name="useMinAndMaxTimes">Whether the notification should be limited within a time box.</param>
        /// <param name="minHour">The minimum hour of the time box.</param>
        /// <param name="minMinute">The minimum minute of the time box.</param>
        /// <param name="maxHour">The maximum hour of the time box.</param>
        /// <param name="maxMinute">The maximum minute of the time box.</param>
        /// <param name="fallbackStrategy">The strategy to be used in case the notification is scheduled outside the desired time box.</param>
        /// <param name="fallbackHour">The fallback hour the notification should be scheduled to. Must be between min and maxHour.</param>
        /// <param name="fallbackMinute">The fallback minute the notification should be scheduled to. Must be between min and maxMinute.</param>
        public static void RegisterNotificationAsPending(INotification notification, string notificationDefinitionName,
            bool rescheduleOnDeviceRestart, bool cancelOnDeviceRestart, double timeTrigger, bool useMinAndMaxTimes,
            int minHour, int minMinute, int maxHour, int maxMinute, TimeBoxFallbackStrategy fallbackStrategy,
            int fallbackHour, int fallbackMinute)
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(PendingNotificationsManager))
                    .AddComponent<PendingNotificationsManager>();
                DontDestroyOnLoad(_instance);
            }

            _instance.InternalRegisterNotificationAsPending(notification, notificationDefinitionName,
                rescheduleOnDeviceRestart, cancelOnDeviceRestart, timeTrigger, useMinAndMaxTimes, minHour, minMinute,
                maxHour, maxMinute, fallbackStrategy, fallbackHour, fallbackMinute);
        }

        /// <summary>
        /// Checks if a notification is pending.
        /// </summary>
        /// <param name="notificationDefinitionName">The name of the corresponding Notification Definition.</param>
        /// <returns>Wether the notification is pending.</returns>
        public static bool IsPending(string notificationDefinitionName)
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(PendingNotificationsManager))
                    .AddComponent<PendingNotificationsManager>();
                DontDestroyOnLoad(_instance);
            }

            return _instance.InternalIsPending(notificationDefinitionName);
        }

        private void InternalRegisterNotificationAsPending(INotification notification,
            string notificationDefinitionName,
            bool rescheduleOnDeviceRestart, bool cancelOnDeviceRestart, double timeTrigger, bool useMinAndMaxTimes,
            int minHour, int minMinute, int maxHour, int maxMinute, TimeBoxFallbackStrategy fallbackStrategy,
            int fallbackHour, int fallbackMinute)
        {
            if (!notification.Id.HasValue)
                return;

            if (!notification.Scheduled)
                return;

            if (notification.DeliveryTime < DateTime.Now)
                return;

            if (_pendingNotifications == null)
            {
                Deserialize();
                _pendingNotifications ??= new List<PendingNotification>();
            }

            _pendingNotifications.Add(new PendingNotification
            {
                Notification = notification,
                NotificationDefinitionName = notificationDefinitionName,
                RescheduleOnDeviceRestart = rescheduleOnDeviceRestart,
                CancelOnDeviceRestart = cancelOnDeviceRestart,
                TimeTrigger = timeTrigger,
                UseTimeBox = useMinAndMaxTimes,
                MinHour = minHour,
                MinMinute = minMinute,
                MaxHour = maxHour,
                MaxMinute = maxMinute,
                FallbackStrategy = fallbackStrategy,
                FallbackHour = fallbackHour,
                FallbackMinute = fallbackMinute
            });

            Serialize();
        }

        private bool InternalIsPending(string notificationDefinitionName)
        {
            Deserialize();

            if (_pendingNotifications == null || _pendingNotifications.Count == 0)
                return false;

            ClearExpiredNotifications();

            return _pendingNotifications.Exists(notification =>
                notification.NotificationDefinitionName.Equals(notificationDefinitionName));
        }

        private void OnDeviceRestarted()
        {
            if (_pendingNotifications == null
                || _pendingNotifications.Count == 0)
                return;

            if (!NotificationPlatformsManager.TryCreateNotificationPlatform(out var platform))
                return;

            ClearExpiredNotifications();
            CancelOrReschedulePendingNotifications(platform);
        }

        private void ClearExpiredNotifications()
        {
            for (var i = _pendingNotifications.Count - 1; i >= 0; --i)
            {
                if (_pendingNotifications[i].Notification.DeliveryTime < DateTime.Now)
                {
                    _pendingNotifications.RemoveAt(i);
                }
            }
        }

        private void CancelOrReschedulePendingNotifications(INotificationPlatform platform)
        {
            foreach (var pendingNotification in _pendingNotifications)
            {
                if (pendingNotification.RescheduleOnDeviceRestart)
                {
                    pendingNotification.Notification.DeliveryTime =
                        NotificationDeliveryTimeCalculator.CalculateDeliveryTime(pendingNotification.TimeTrigger,
                            pendingNotification.UseTimeBox, pendingNotification.MinHour,
                            pendingNotification.MinMinute, pendingNotification.MaxHour, pendingNotification.MaxMinute,
                            pendingNotification.FallbackStrategy, pendingNotification.FallbackHour,
                            pendingNotification.FallbackMinute);

                    platform.RescheduleNotification(pendingNotification.Notification);
                }

                if (pendingNotification.CancelOnDeviceRestart)
                {
                    platform.CancelNotification(pendingNotification.Notification);
                }
            }
        }

        private void Serialize()
        {
            var serializer = new PendingNotificationPlayerPrefsSerializer();
            serializer.Serialize(_pendingNotifications, PendingNotificationPlayerPrefsSerializer.DefaultPlayerPrefsKey);
        }

        private void Deserialize()
        {
            if (!NotificationPlatformsManager.TryCreateNotificationPlatform(out var platform))
                return;

            var serializer = new PendingNotificationPlayerPrefsSerializer();
            _pendingNotifications = serializer.Deserialize(platform,
                PendingNotificationPlayerPrefsSerializer.DefaultPlayerPrefsKey);
        }
    }
}
