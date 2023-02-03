#if UNITY_IOS
using System;
using Unity.Notifications.iOS;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    public class iOSNotificationPlatform : INotificationPlatform<iOSNotification>
    {
        /// <inheritdoc />
        INotification INotificationPlatform.CreateNotification()
        {
            return CreateNotification();
        }

        /// <inheritdoc />
        public iOSNotification CreateNotification()
        {
            return new iOSNotification();
        }

        /// <inheritdoc />
        public void ScheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            ScheduleNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void ScheduleNotification(iOSNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            iOSNotificationCenter.ScheduleNotification(notification.InternalNotification);
            notification.OnScheduled();
        }

        /// <inheritdoc />
        public void RescheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            RescheduleNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void RescheduleNotification(iOSNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (!notification.Id.HasValue)
            {
                throw new InvalidOperationException("Unable to reschedule a notification without an Id.");
            }

            var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();

            foreach (var scheduledNotification in scheduledNotifications)
            {
                iOSNotificationCenter.ScheduleNotification(
                    scheduledNotification.Identifier != notification.Id.ToString()
                        ? scheduledNotification
                        : notification.InternalNotification);
            }
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            CancelScheduledNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(iOSNotification notification)
        {
            if (notification is not {Scheduled: true} || notification.Id == null)
                return;

            iOSNotificationCenter.RemoveScheduledNotification(notification.Id.Value.ToString());
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(int notificationId)
        {
            iOSNotificationCenter.RemoveScheduledNotification(notificationId.ToString());
        }

        /// <inheritdoc />
        public void CancelAllScheduledNotifications()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            ClearDisplayedNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(iOSNotification notification)
        {
            if (notification is not {Scheduled: true} || notification.Id == null)
                return;

            if (notification.DeliveryTime < DateTime.Now)
                iOSNotificationCenter.RemoveDeliveredNotification(notification.Id.Value.ToString());
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(int notificationId)
        {
            iOSNotificationCenter.RemoveDeliveredNotification(notificationId.ToString());
        }

        /// <inheritdoc />
        public void ClearAllDisplayedNotifications()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }

        /// <inheritdoc />
        public bool TryGetLastRespondedNotificationData(out INotification notification)
        {
            var internalNotification = iOSNotificationCenter.GetLastRespondedNotification();

            if (internalNotification != null)
            {
                var iOSNotification = new iOSNotification(internalNotification);
                notification = iOSNotification;
                return true;
            }

            notification = null;
            return false;
        }
    }
}
#endif
