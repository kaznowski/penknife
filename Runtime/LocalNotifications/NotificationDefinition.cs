using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// ScriptableObject used to define how a notification should be triggered, when it should be scheduled and what data it should contain.
    /// </summary>
    [CreateAssetMenu(fileName = "NotificationDefinition",
        menuName = "DoubleDash/MobileNotifications/NotificationDefinition")]
    public class NotificationDefinition : ScriptableObject
    {
        [Tooltip("Dê um nome único a esta definição de notificação.")] [SerializeField]
        private string _name;

        [Tooltip("Caso falso, esta notificação não será agendada, mesmo que o Game Event seja engatilhado.")]
        [SerializeField]
        private bool _enabled = true;

        [Tooltip("Em qual grupo essa notificação deve ser colocada?")] [SerializeField]
        private string _group = "default_group";

        [Tooltip("Quais dados essa notificação deve armazenar? Quando o usuário clicar nela, é possível recuperá-los." +
                 "Só funciona em notificações com TimeTrigger diferente de 0.")]
        [SerializeField]
        private string _data;

        [Header("Text")] [Tooltip("Título exibido na notificação.")] [SerializeField]
        private NotificationLocalizedString _title;

        [Tooltip("Subtítulo exibido na notificação. Só funciona em iOS.")] [SerializeField]
        private NotificationLocalizedString _subtitle;

        [Tooltip("Texto exibido na notificação.")] [SerializeField]
        private NotificationLocalizedString _body;

        [Header("Time")] [Tooltip("A notificação deverá ser agendada para daqui a quantos minutos?")] [SerializeField]
        private double _timeTrigger;

        [Tooltip("A notificação deve se repetir diariamente no mesmo horário?")] [SerializeField]
        private bool _repeats;

        [Header("Timebox")]
        [Tooltip("Devo forçar que a notificação seja agendada dentro de uma faixa de horário?")]
        [SerializeField]
        private bool _limitToTimeBox;

        [Tooltip(
            "Devo forçar que a notificação seja agendada dentro de uma faixa de horário? Horas no formato 00~23h.")]
        [SerializeField]
        private int _minHour;

        [Tooltip("Devo forçar que a notificação seja agendada dentro de uma faixa de horário? Minutos de 00~59.")]
        [SerializeField]
        private int _minMinute;

        [Tooltip(
            "Devo forçar que a notificação seja agendada dentro de uma faixa de horário? Horas no formato 00~23h.")]
        [SerializeField]
        private int _maxHour;

        [Tooltip("Devo forçar que a notificação seja agendada dentro de uma faixa de horário? Minutos de 00~59.")]
        [SerializeField]
        private int _maxMinute;

        [Tooltip("O que devo fazer caso a notificação seja agendada fora da faixa de horário configurada?" +
                 " Devo atrasá-la para a próxima faixa ou adiantá-la para a faixa anterior?" +
                 " Caso opte por adiantar a notificação e a faixa de horário anterior já tenha passado," +
                 " ela será agendada para a próxima faixa disponível.")]
        [SerializeField]
        private TimeBoxFallbackStrategy _timeBoxFallbackStrategy;

        [Tooltip(
            "Caso a notificação seja agendada fora do horário configurado, em qual horário da faixa fallback você deseja que ela seja agendada?" +
            " Deve estar entre Min Hour e Max Hour.")]
        [SerializeField]
        private int _fallbackHour;

        [Tooltip(
            "Caso a notificação seja agendada fora do horário configurado, em qual horário da faixa fallback você deseja que ela seja agendada?" +
            " Deve estar entre Min Minute e Max Minute.")]
        [SerializeField]
        private int _fallbackMinute;

        [Header("Trigger")]
        [Tooltip("A notificação deve evitar ser disparada caso já exista uma duplicata dela pendente?")]
        [SerializeField]
        private bool _dontTriggerIfPending = true;

        [Tooltip("A notificação deve se limitar a ser lançada somente uma vez?")] [SerializeField]
        private bool _triggerOnlyOnce;

        [Tooltip("A notificação deve ser reagendada quando a jogadora reabrir o jogo enquanto ela estiver pendente?")]
        [SerializeField]
        private bool _rescheduleOnDeviceRestart;

        [Tooltip("A notificação deve ser cancelada quando a jogadora reabrir o jogo enquanto ela estiver pendente?")]
        [SerializeField]
        private bool _cancelOnDeviceRestart;

        [Header("Icons")]
        [Tooltip(
            "Ícone custom exibido na notificação. Deve ser o mesmo ID configurado em Edit > Project Settings > Mobile Notifications." +
            " Só funciona em Android.")]
        [SerializeField]
        private string _largeIconId;

        [Tooltip(
            "Ícone custom exibido na notificação. Deve ser o mesmo ID configurado em Edit > Project Settings > Mobile Notifications." +
            " Só funciona em Android.")]
        [SerializeField]
        private string _smallIconId;

        public string NotificationDefinitionName => _name;

        private INotification _notification;
        private INotificationPlatform _notificationPlatform;

        public void ScheduleNotification()
        {
            if (!_enabled)
                return;

            if (_triggerOnlyOnce && NotificationDefinitionHistoryManager.HasNotificationDefinitionBeenTriggered(_name))
                return;

            if (_dontTriggerIfPending && PendingNotificationsManager.IsPending(_name))
                return;

            if (_notificationPlatform == null)
            {
                if (!NotificationPlatformsManager.TryCreateNotificationPlatform(out var platform)) return;
                _notificationPlatform = platform;
            }

            if (_notification == null)
            {
                _notification = _notificationPlatform.CreateNotification();

                SetNotificationText(_notification);
                SetNotificationCustomIcon(_notification);
                SetNotificationGroup(_notification);
                SetNotificationData(_notification);
            }

            SetNotificationDeliveryTime(_notification);

            _notificationPlatform.ScheduleNotification(_notification);

            PendingNotificationsManager.RegisterNotificationAsPending(_notification, _name, _rescheduleOnDeviceRestart,
                _cancelOnDeviceRestart, _timeTrigger, _limitToTimeBox, _minHour, _minMinute, _maxHour, _maxMinute,
                _timeBoxFallbackStrategy, _fallbackHour, _fallbackMinute);

            NotificationDefinitionHistoryManager.RegisterTriggeredNotificationDefinition(this);

            Debug.Log($"Just scheduled notification {_notification.Id} {_name}" +
                      $"\nTitle: {_notification.Title}" +
                      $"\nBody: {_notification.Body}");
        }

        private void SetNotificationText(INotification notification)
        {
            if (_title != null)
                notification.Title = _title.GetString();

            if (_subtitle != null)
                notification.Subtitle = _subtitle.GetString();

            if (_body != null)
                notification.Body = _body.GetString();
        }

        private void SetNotificationCustomIcon(INotification notification)
        {
            if (!string.IsNullOrEmpty(_smallIconId))
            {
                notification.SmallIconId = _smallIconId;
            }

            if (!string.IsNullOrEmpty(_largeIconId))
            {
                notification.LargeIconId = _largeIconId;
            }
        }

        private void SetNotificationGroup(INotification notification)
        {
            notification.Group = _group;
        }

        private void SetNotificationData(INotification notification)
        {
            notification.Data = _data;
        }

        private void SetNotificationDeliveryTime(INotification notification)
        {
            notification.Repeats = _repeats;

            notification.DeliveryTime = NotificationDeliveryTimeCalculator.CalculateDeliveryTime(_timeTrigger,
                _limitToTimeBox, _minHour, _maxMinute, _maxHour, _maxMinute,
                _timeBoxFallbackStrategy, _fallbackHour, _fallbackMinute);
        }
    }
}
