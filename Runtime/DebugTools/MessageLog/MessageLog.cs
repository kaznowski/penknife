using UnityEngine;
using DoubleDash.CodingTools.GenericDatabases;

namespace DoubleDash.CodingTools.DebugTools 
{
    public interface IMessageLog 
    {
        public void TriggerMessage(string messageName);

        public MessageLog.Message GetMessageByName(string messageName);

        public void WriteMessageLog(string text);

        public void WriteMessageWarning(string text);

        public void WriteMessageError(string text);
    }

    [System.Serializable]
    public class MessageLog : IMessageLog
    {
        [System.Serializable]
        public class Message : INameable
        {
            [Tooltip("Name (identification) of the debug message to be sent.")]
            public string name;
            [Tooltip("Type of the message to be shown.")]
            public MessageType messageType;
            [Tooltip("Text within the message."), TextArea]
            public string text;

            public Message(string name, string text, MessageType messageType) {
                this.name = name;
                this.text = text;
                this.messageType = messageType;
            }

            public string Name { get => name; set => name = value; }

            public enum MessageType { 
                log,
                warning,
                error
            }
        }

        [Tooltip("Pre-written messages that can be shown. These are indexed by name.")]
        public Database<Message> messages;

        #region Public Functions

        public void TriggerMessage(string messageName)
        {
            //Get the appropriate message and print it.
            Message targetMessage = GetMessageByName(messageName);
            PrintMessage(targetMessage);
        }

        public Message GetMessageByName(string messageName) 
        {
            //Get the appropriate message
            Message currentMessage = messages.GetEntry(messageName);

            //If the message was found, return it.
            if (currentMessage != null) return currentMessage;

            //If the message wasn't found, return a default error message.
            return new Message("MessageLogError", "A message with name '" + messageName + "' couldn't be found within MessageLog.", Message.MessageType.error);
        }

        public void WriteMessageLog(string text) => PrintMessage(new Message("NewDynamicMessageLog", text, Message.MessageType.log));

        public void WriteMessageWarning(string text) => PrintMessage(new Message("NewDynamicMessageWarning", text, Message.MessageType.warning));

        public void WriteMessageError(string text) => PrintMessage(new Message("NewDynamicMessageError", text, Message.MessageType.error));

        #endregion

        void PrintMessage(Message targetMessage) 
        {
            //Print
            switch (targetMessage.messageType)
            {
                case (Message.MessageType.log):
                    Debug.Log(targetMessage.text);
                    break;
                case (Message.MessageType.warning):
                    Debug.LogWarning(targetMessage.text);
                    break;
                case (Message.MessageType.error):
                    Debug.LogError(targetMessage.text);
                    break;
            }
        }
    }
}

