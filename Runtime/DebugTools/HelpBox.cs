using UnityEngine;
using System;
namespace DoubleDash.CodingTools.DebugTools 
{
    public enum HelpBoxType
    {
        /// <summary>
        ///   <para>Neutral message.</para>
        /// </summary>
        None,
        /// <summary>
        ///   <para>Info message.</para>
        /// </summary>
        Info,
        /// <summary>
        ///   <para>Warning message.</para>
        /// </summary>
        Warning,
        /// <summary>
        ///   <para>Error message.</para>
        /// </summary>
        Error
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public HelpBoxAttribute(string text, string docsUrl = null, HelpBoxType type = HelpBoxType.Info)
        {
            Text = text;
            DocsUrl = docsUrl;
            Type = type;
        }

        public string Text { get; }
        public string DocsUrl { get; }
        public HelpBoxType Type { get; }
    }
}

    /*
    [System.Serializable]
    public class HelpBox
    {
        public enum HelpBoxMessageType { 
            log,
            warning,
            error
        }

        public string text = "Write down your help text here!";
        public HelpBoxMessageType type;
        public int height = 32;

        public HelpBox (string text, HelpBoxMessageType type, int height)
        {
            this.text = text;
            this.type = type;
            this.height = height;
        }
    }
    */
