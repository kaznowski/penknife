#if UNITY_ANDROID
using System;
using System.Linq;
using Unity.Notifications.Android;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    public class AndroidNotificationPlatform : INotificationPlatform<AndroidNotification>
    {
        private const string DefaultChannelId = "default_group";
        private const string DefaultChannelName = "Default Channel";
        private const Importance DefaultChannelImportance = Importance.Default;
        private const string DefaultChannelDescription = "Generic notifications";

        public AndroidNotificationPlatform()
        {
            if (ExistsChannel(DefaultChannelId)) return;

            var defaultChannel = new AndroidNotificationChannel
            {
                Id = DefaultChannelId,
                Name = DefaultChannelName,
                Importance = DefaultChannelImportance,
                Description = DefaultChannelDescription
            };

            AndroidNotificationCenter.RegisterNotificationChannel(defaultChannel);
        }

        /// <inheritdoc />
        INotification INotificationPlatform.CreateNotification()
        {
            return CreateNotification();
        }

        /// <inheritdoc />
        public AndroidNotification CreateNotification()
        {
            return new AndroidNotification();
        }

        /// <inheritdoc />
        public void ScheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not AndroidNotification androidNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to AndroidNotificationPlatform isn't an AndroidNotification.");
            }

            ScheduleNotification(androidNotification);
        }

        /// <inheritdoc />
        public void ScheduleNotification(AndroidNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (!ExistsChannel(notification.Group))
            {
                var channel = new AndroidNotificationChannel
                {
                    Id = notification.Group,
                    Name = notification.Group,
                    Importance = DefaultChannelImportance,
                    Description = DefaultChannelDescription
                };
                AndroidNotificationCenter.RegisterNotificationChannel(channel);
            }

            if (notification.Id.HasValue)
            {
                AndroidNotificationCenter.SendNotificationWithExplicitID(notification.InternalNotification,
                    notification.Group,
                    notification.Id.Value);
            }
            else
            {
                var notificationId = AndroidNotificationCenter.SendNotification(notification.InternalNotification,
                    notification.Group);
                notification.Id = notificationId;
            }

            notification.OnScheduled();
        }

        /// <inheritdoc />
        public void RescheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not AndroidNotification androidNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to AndroidNotificationPlatform isn't an AndroidNotification.");
            }

            RescheduleNotification(androidNotification);
        }

        /// <inheritdoc />
        public void RescheduleNotification(AndroidNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (!notification.Id.HasValue)
            {
                throw new InvalidOperationException("Unable to reschedule a notification without an Id.");
            }

            AndroidNotificationCenter.UpdateScheduledNotification(notification.Id.Value,
                notification.InternalNotification,
                notification.Group);
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not AndroidNotification androidNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to AndroidNotificationPlatform isn't an AndroidNotification.");
            }

            CancelScheduledNotification(androidNotification);
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(AndroidNotification notification)
        {
            if (notification is not {Scheduled: true} || notification.Id == null)
                return;

            AndroidNotificationCenter.CancelScheduledNotification(notification.Id.Value);
        }

        /// <inheritdoc />
        public void CancelScheduledNotification(int notificationId)
        {
            AndroidNotificationCenter.CancelScheduledNotification(notificationId);
        }

        /// <inheritdoc />
        public void CancelAllScheduledNotifications()
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification is not AndroidNotification androidNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to AndroidNotificationPlatform isn't an AndroidNotification.");
            }

            ClearDisplayedNotification(androidNotification);
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(AndroidNotification notification)
        {
            if (notification is not {Id: { }})
                return;

            AndroidNotificationCenter.CancelDisplayedNotification(notification.Id.Value);
        }

        /// <inheritdoc />
        public void ClearDisplayedNotification(int notificationId)
        {
            AndroidNotificationCenter.CancelDisplayedNotification(notificationId);
        }

        /// <inheritdoc />
        public void ClearAllDisplayedNotifications()
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }

        /// <inheritdoc />
        public bool TryGetLastRespondedNotificationData(out INotification notification)
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData != null)
            {
                var internalNotification = notificationIntentData.Notification;
                var androidNotification = new AndroidNotification(internalNotification)
                {
                    Id = notificationIntentData.Id
                };
                notification = androidNotification;
                return true;
            }

            notification = null;
            return false;
        }

        private bool ExistsChannel(string channelId)
        {
            var channels = AndroidNotificationCenter.GetNotificationChannels().ToList();
            return channels.Exists(channel => channel.Id.Equals(channelId));
        }
    }
}
#endif
