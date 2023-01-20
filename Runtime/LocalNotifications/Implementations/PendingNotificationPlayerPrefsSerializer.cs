using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    public class PendingNotificationPlayerPrefsSerializer : IPendingNotificationSerializer
    {
        /// <summary>
        /// Default Player Prefs key used to save the pending notifications.
        /// </summary>
        public const string DefaultPlayerPrefsKey = "DoubleDash.PendingNotifications";

        /// <inheritdoc />
        public void Serialize(List<PendingNotification> pendingNotifications, string path)
        {
            if (pendingNotifications == null || pendingNotifications.Count == 0)
            {
                PlayerPrefs.SetString(path, "");
                return;
            }

            var pendingNotificationData = new PendingNotificationJsonData();

            foreach (var pendingNotification in pendingNotifications)
            {
                pendingNotificationData.Data.Add(CreatePendingNotificationData(pendingNotification));
            }

            var json = JsonUtility.ToJson(pendingNotificationData);
            PlayerPrefs.SetString(path, json);
        }

        /// <inheritdoc />
        public List<PendingNotification> Deserialize(INotificationPlatform platform, string path)
        {
            var json = PlayerPrefs.GetString(path, "");

            if (string.IsNullOrEmpty(json))
                return null;

            var pendingNotificationData = JsonUtility.FromJson<PendingNotificationJsonData>(json);

            var pendingNotifications = new List<PendingNotification>();

            foreach (var data in pendingNotificationData.Data)
            {
                pendingNotifications.Add(ReadPendingNotificationData(platform, data));
            }

            return pendingNotifications;
        }

        private static PendingNotificationJsonData.PendingNotificationSerializedJsonData CreatePendingNotificationData(
            PendingNotification pendingNotification)
        {
            var pendingNotificationData = new PendingNotificationJsonData.PendingNotificationSerializedJsonData();

            if (pendingNotification.Notification.Id.HasValue)
                pendingNotificationData.Id = pendingNotification.Notification.Id.Value;
            pendingNotificationData.Title = pendingNotification.Notification.Title;
            pendingNotificationData.Body = pendingNotification.Notification.Body;
            pendingNotificationData.Subtitle = pendingNotification.Notification.Subtitle;
            pendingNotificationData.Data = pendingNotification.Notification.Data;
            pendingNotificationData.Group = pendingNotification.Notification.Group;
            if (pendingNotification.Notification.DeliveryTime.HasValue)
                pendingNotificationData.DeliveryTime = pendingNotification.Notification.DeliveryTime.Value;
            pendingNotificationData.Repeats = pendingNotification.Notification.Repeats;
            pendingNotificationData.Scheduled = pendingNotification.Notification.Scheduled;
            pendingNotificationData.LargeIconId = pendingNotification.Notification.LargeIconId;
            pendingNotificationData.SmallIconId = pendingNotification.Notification.SmallIconId;
            pendingNotificationData.NotificationDefinitionName = pendingNotification.NotificationDefinitionName;
            pendingNotificationData.RescheduleOnDeviceRestart = pendingNotification.RescheduleOnDeviceRestart;
            pendingNotificationData.CancelOnDeviceRestart = pendingNotification.CancelOnDeviceRestart;
            pendingNotificationData.TimeTrigger = pendingNotification.TimeTrigger;
            pendingNotificationData.UseTimeBox = pendingNotification.UseTimeBox;
            pendingNotificationData.MinHour = pendingNotification.MinHour;
            pendingNotificationData.MinMinute = pendingNotification.MinMinute;
            pendingNotificationData.MaxHour = pendingNotification.MaxHour;
            pendingNotificationData.MaxMinute = pendingNotification.MaxMinute;
            pendingNotificationData.FallbackStrategy = pendingNotification.FallbackStrategy;
            pendingNotificationData.FallbackHour = pendingNotification.FallbackHour;
            pendingNotificationData.FallbackMinute = pendingNotification.FallbackMinute;

            return pendingNotificationData;
        }

        private static PendingNotification ReadPendingNotificationData(INotificationPlatform platform,
            PendingNotificationJsonData.PendingNotificationSerializedJsonData pendingNotificationData)
        {
            var notification = platform.CreateNotification();

            notification.Id = pendingNotificationData.Id;
            notification.Title = pendingNotificationData.Title;
            notification.Body = pendingNotificationData.Body;
            notification.Subtitle = pendingNotificationData.Subtitle;
            notification.Data = pendingNotificationData.Data;
            notification.Group = pendingNotificationData.Group;
            notification.DeliveryTime = pendingNotificationData.DeliveryTime;
            notification.Repeats = pendingNotificationData.Repeats;
            notification.Scheduled = pendingNotificationData.Scheduled;
            notification.LargeIconId = pendingNotificationData.LargeIconId;
            notification.SmallIconId = pendingNotificationData.SmallIconId;


            return new PendingNotification
            {
                Notification = notification,
                NotificationDefinitionName = pendingNotificationData.NotificationDefinitionName,
                RescheduleOnDeviceRestart = pendingNotificationData.RescheduleOnDeviceRestart,
                CancelOnDeviceRestart = pendingNotificationData.CancelOnDeviceRestart,
                TimeTrigger = pendingNotificationData.TimeTrigger,
                UseTimeBox = pendingNotificationData.UseTimeBox,
                MinHour = pendingNotificationData.MinHour,
                MinMinute = pendingNotificationData.MinMinute,
                MaxHour = pendingNotificationData.MaxHour,
                MaxMinute = pendingNotificationData.MaxMinute,
                FallbackStrategy = pendingNotificationData.FallbackStrategy,
                FallbackHour = pendingNotificationData.FallbackHour,
                FallbackMinute = pendingNotificationData.FallbackMinute
            };
        }
    }

    /// <summary>
    /// Data structure used by the PendingNotificationPlayerPrefsSerializer.
    /// </summary>
    [Serializable]
    public class PendingNotificationJsonData
    {
        public List<PendingNotificationSerializedJsonData> Data = new List<PendingNotificationSerializedJsonData>();

        [Serializable]
        public class PendingNotificationSerializedJsonData
        {
            public int Id;
            public string Title;
            public string Body;
            public string Subtitle;
            public string Data;
            public string Group;
            public JsonDateTime DeliveryTime;
            public bool Repeats;
            public bool Scheduled;
            public string LargeIconId;
            public string SmallIconId;

            public string NotificationDefinitionName;
            public bool RescheduleOnDeviceRestart;
            public bool CancelOnDeviceRestart;
            public double TimeTrigger;
            public bool UseTimeBox;
            public int MinHour;
            public int MinMinute;
            public int MaxHour;
            public int MaxMinute;
            public TimeBoxFallbackStrategy FallbackStrategy;
            public int FallbackHour;
            public int FallbackMinute;
        }
    }

    /// <summary>
    /// Solution to serializing DateTime in Json.
    /// </summary>
    [Serializable]
    public struct JsonDateTime
    {
        public long Value;

        public static implicit operator DateTime(JsonDateTime jsonDateTime)
        {
            return DateTime.FromFileTimeUtc(jsonDateTime.Value);
        }

        public static implicit operator JsonDateTime(DateTime dateTime)
        {
            var jsonDateTime = new JsonDateTime
            {
                Value = dateTime.ToFileTimeUtc()
            };

            return jsonDateTime;
        }
    }
}
