using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    public class NotificationDefinitionHistoryPlayerPrefsSerializer : INotificationDefinitionHistorySerializer
    {
        /// <summary>
        /// Default Player Prefs key used to save the collection of triggered notifications.
        /// </summary>
        public const string DefaultPlayerPrefsKey = "DoubleDash.NotificationDefinitionHistory";

        /// <inheritdoc />
        public List<NotificationDefinitionHistory> Deserialize(string path)
        {
            var json = PlayerPrefs.GetString(path, "");

            if (string.IsNullOrEmpty(json))
                return null;

            var jsonData = JsonUtility.FromJson<NotificationDefinitionHistoryJsonData>(json);

            var notificationDefinitionHistories = new List<NotificationDefinitionHistory>();

            foreach (var name in jsonData.NotificationDefinitionNames)
            {
                notificationDefinitionHistories.Add(new NotificationDefinitionHistory
                {
                    NotificationDefinitionName = name
                });
            }

            return notificationDefinitionHistories;
        }

        /// <inheritdoc />
        public void Serialize(List<NotificationDefinitionHistory> notificationDefinitionHistories, string path)
        {
            if (notificationDefinitionHistories == null
                || notificationDefinitionHistories.Count == 0)
            {
                PlayerPrefs.SetString(path, "");
                return;
            }

            var jsonData = new NotificationDefinitionHistoryJsonData
            {
                NotificationDefinitionNames = new List<string>()
            };

            foreach (var history in notificationDefinitionHistories)
            {
                jsonData.NotificationDefinitionNames.Add(history.NotificationDefinitionName);
            }

            var json = JsonUtility.ToJson(jsonData);

            PlayerPrefs.SetString(path, json);
        }
    }

    /// <summary>
    /// Data structure used by the NotificationDefinitionHistoryPlayerPrefsSerializer.
    /// </summary>
    [System.Serializable]
    public class NotificationDefinitionHistoryJsonData
    {
        public List<string> NotificationDefinitionNames;
    }
}
