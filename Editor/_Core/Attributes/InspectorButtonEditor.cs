 using System.Collections.Generic;
 using System.Linq;
 using System.Reflection;
 using UnityEditor;
 using UnityEngine;

 
public abstract class InspectorButtonEditor : Editor
    {
        private class Button
        {
            public MethodInfo method;
            public string title;
            public ExecutionMode mode;
        }

        private IList<Button> buttons = new List<Button>();

        protected virtual void OnEnable()
        {
            CaptureButtons();
        }

        protected virtual void OnDisable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawButtons();
        }

        private void CaptureButtons()
        {
            var bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            if (target == null)
                return;
            var methods = target.GetType()
                                .GetMethods(bindings)
                                .Where(m => HasSingleAttribute(m) &&
                                            HasNoParameters(m) &&
                                            !m.ContainsGenericParameters);

            foreach (var method in methods)
            {
                var button = new Button();
                button.method = method;
                var attrs = GetAttributes(method);
                var attr = attrs[0] as InspectorButtonAttribute;
                if (attr != null)
                {
                    button.title = attr.title;
                    button.mode = attr.mode;
                }

                if (string.IsNullOrEmpty(button.title))
                    button.title = ObjectNames.NicifyVariableName(method.Name);

                buttons.Add(button);
            }
        }

        private bool HasSingleAttribute(MethodInfo method)
        {
            var attrs = GetAttributes(method);
            return attrs.Length == 1;
        }

        private bool HasNoParameters(MethodInfo method)
        {
            var parameters = method.GetParameters();
            return parameters.Length == 0;
        }

        private object[] GetAttributes(MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(InspectorButtonAttribute), true);
        }

        private void DrawButtons()
        {
            bool isPlaying = Application.isPlaying;
            for (int i = 0, count = buttons.Count; i < count; ++i)
            {
                var button = buttons[i];
                bool disabled = (ExecutionMode.Editor == button.mode && isPlaying) ||
                                (ExecutionMode.Runtime == button.mode && !isPlaying);

                using (var group = new EditorGUI.DisabledScope(disabled))
                {
                    if (GUILayout.Button(button.title))
                    {
                        button.method.Invoke(target, null);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Custom editor for all MonoBehaviour scripts in order to draw buttons for all button attributes (<see cref="ButtonAttribute"/>).
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    [CanEditMultipleObjects]
    public sealed class MonoBehaviourButtonEditor : InspectorButtonEditor
    { }

    /// <summary>
    /// Custom editor for all ScriptableObject scripts in order to draw buttons for all button attributes (<see cref="ButtonAttribute"/>).
    /// </summary>
    [CustomEditor(typeof(ScriptableObject), true, isFallback = false)]
    [CanEditMultipleObjects]
    public sealed class ScriptableObjectButtonEditor : InspectorButtonEditor
    { }