using UnityEngine;

namespace DoubleDash.CodingTools.DebugTools
{
    [CreateAssetMenu(fileName = "NewMessageLog", menuName = "DoubleDash/Debug/MessageLogs")]
    public class MessageLogScriptable : ScriptableObject
    {
        public MessageLog messageLog;

        public MessageLog.Message GetMessageByName(string messageName) => messageLog.GetMessageByName(messageName);

        public void TriggerMessage(string messageName) => messageLog.TriggerMessage(messageName);

        public void WriteMessageError(string text) => messageLog.WriteMessageError(text);

        public void WriteMessageLog(string text) => messageLog.WriteMessageLog(text);

        public void WriteMessageWarning(string text) => messageLog.WriteMessageWarning(text);
    }
}
