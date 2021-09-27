using UnityEngine;
using UnityEditor;
using System;

namespace DoubleDash.CodingTools.DebugTools
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
        const float XPadding = 30f;
        const float YPadding = 5f;
        const float DefaultHeight = 20f;
        const float DocsButtonHeight = 20f;
        float _height;

        public override void OnGUI(Rect position)
        {

            var attr = attribute as HelpBoxAttribute;
            CalculateHeight(attr);

            EditorGUI.HelpBox(position, attr.Text, (MessageType)attr.Type);

            if (!string.IsNullOrEmpty(attr.DocsUrl))
            {
                position = new Rect(
                    position.x + position.width - 40,
                    position.y + position.height - DocsButtonHeight,
                    40,
                    DocsButtonHeight);


                if (GUI.Button(position, "Docs"))
                {
                    if (attr.DocsUrl.StartsWith("http"))
                        Application.OpenURL(attr.DocsUrl);
                    else
                        Application.OpenURL($"https://docs.unity3d.com/ScriptReference/{attr.DocsUrl}");
                }
            }
        }

        
        public override float GetHeight()
        {
            var attr = attribute as HelpBoxAttribute;

            return CalculateHeight(attr);
        }
        

        float CalculateHeight(HelpBoxAttribute attr)
        {
            _height = (attr.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Length + 1) * DefaultHeight;

            if (!string.IsNullOrEmpty(attr.DocsUrl))
                _height += DocsButtonHeight;

            return _height;
        }
    }
}