using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using DoubleDash.CodingTools.ClassExtensions;
using DoubleDash.ComponentManagement.AnimatorManagement;

/*

namespace DoubleDash.CodingTools.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorManagerParameter))]
    public class AnimatorManagerParameterDrawer : PropertyDrawer
    {
        int padding = 2;

        //Override the property's ONGUI draw
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Set up the position
            //label = EditorGUI.BeginProperty(position, label, property);
            //position = EditorGUI.PrefixLabel(position, label);

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            //Begin looking for changes that will cause updates to the serialized properties.
            EditorGUI.BeginChangeCheck();

            //Get name and type
            SerializedProperty parameterName = property.FindPropertyRelative("parameterName");
            SerializedProperty parameterType = property.FindPropertyRelative("parameterType");

            //Get possible values
            SerializedProperty boolValue  = property.FindPropertyRelative("boolValue");
            SerializedProperty intValue   = property.FindPropertyRelative("intValue");
            SerializedProperty floatValue = property.FindPropertyRelative("floatValue");

            //Get copy of the stuct
            Rect nameRect = position;

            //Draw name and type
            EditorGUI.PropertyField(nameRect, parameterName, new GUIContent("Name"));

            AddSpace(ref position);

            EditorGUI.PropertyField(position, parameterType, new GUIContent("Type"));

            AddSpace(ref position);

            //Set indentation and end property
            EditorGUI.EndChangeCheck();
            EditorGUI.EndProperty();
        }

        void AddSpace(ref Rect position) 
        {
            float height = position.height + padding;
            position.yMin += height;
            position.yMax += height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            SerializedProperty parameterType = property.FindPropertyRelative("parameterType");

            //If the enum is a Trigger
            if (parameterType.enumValueIndex == 3)
            {
                return (base.GetPropertyHeight(property, label) * 2) + (padding * 1);
            }
            else 
            {
                return (base.GetPropertyHeight(property, label) * 3) + (padding * 2);
            }
        }
    }
}

*/