using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using DoubleDash.CodingTools.ClassExtensions;
using DoubleDash.CodingTools.Editor;

[CustomPropertyDrawer(typeof(SerializeProperty))]
public class SerializePropertyAttributeDrawer : PropertyDrawer
{
    private PropertyInfo propertyFieldInfo = null;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Cache the type that will be used.
        Type fieldType = fieldInfo.FieldType;

        if (fieldType.IsClass && !fieldType.ImplementsOrInherits(typeof(UnityEngine.Object)))
        {
            //Debug.Log(EditorGUI.GetPropertyHeight(property, label, true));

            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            //Debug.Log(base.GetPropertyHeight(property, label));

            return base.GetPropertyHeight(property, label);
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get object being rendered by this
        UnityEngine.Object target = property.serializedObject.targetObject;

        // Get parent of this property
        object serializedPropertyOwner = SerializedPropertyFindOwner.GetSerializedPropertyOwner(property);

        // Cache the type that will be used.
        Type fieldType = fieldInfo.FieldType;

        //Get Property from class that contains that property, and the serialized object
        if (propertyFieldInfo == null)
            propertyFieldInfo = serializedPropertyOwner.GetType().GetProperty(((SerializeProperty)attribute).PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        //If the property was found
        if (propertyFieldInfo != null)
        {
            // Retrieve the value of the property by calling the "Get" method associated to the property:
            object value = propertyFieldInfo.GetValue(serializedPropertyOwner);

            // If the object is not a structure, call default drawer and stop
            if (fieldType.IsClass && !fieldType.ImplementsOrInherits(typeof(UnityEngine.Object))) 
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            // Draw the property, checking for changes:
            EditorGUI.BeginChangeCheck();

            //Draw property and update value
            value = DrawProperty(position, property.propertyType, propertyFieldInfo.PropertyType, value, label);//EditorGUI.PropertyField(position, property, label);

            // If any changes were detected, call the property setter:
            if (EditorGUI.EndChangeCheck() && propertyFieldInfo != null)
            {
                // Record object state for undo:
                Undo.RecordObject(target, "Inspector");

                //Use setter
                propertyFieldInfo.SetValue(serializedPropertyOwner, value);
            }
        }
        else
        {
            EditorGUI.LabelField(position, "Error: could not retrieve property.");
        }
    }

    private object DrawProperty(Rect position, SerializedPropertyType propertyType, Type type, object value, GUIContent label)
    {
        switch (propertyType)
        {
            case SerializedPropertyType.Integer:
                return EditorGUI.IntField(position, label, (int)value);
            case SerializedPropertyType.Boolean:
                return EditorGUI.Toggle(position, label, (bool)value);
            case SerializedPropertyType.Float:
                return EditorGUI.FloatField(position, label, (float)value);
            case SerializedPropertyType.String:
                return EditorGUI.TextField(position, label, (string)value);
            case SerializedPropertyType.Color:
                return EditorGUI.ColorField(position, label, (Color)value);
            case SerializedPropertyType.ObjectReference:
                return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
            case SerializedPropertyType.ExposedReference:
                return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
            case SerializedPropertyType.LayerMask:
                return EditorGUI.LayerField(position, label, (int)value);
            case SerializedPropertyType.Enum:
                return EditorGUI.EnumPopup(position, label, (Enum)value);
            case SerializedPropertyType.Vector2:
                return EditorGUI.Vector2Field(position, label, (Vector2)value);
            case SerializedPropertyType.Vector3:
                return EditorGUI.Vector3Field(position, label, (Vector3)value);
            case SerializedPropertyType.Vector4:
                return EditorGUI.Vector4Field(position, label, (Vector4)value);
            case SerializedPropertyType.Rect:
                return EditorGUI.RectField(position, label, (Rect)value);
            case SerializedPropertyType.AnimationCurve:
                return EditorGUI.CurveField(position, label, (AnimationCurve)value);
            case SerializedPropertyType.Bounds:
                return EditorGUI.BoundsField(position, label, (Bounds)value);
            default:
                throw new NotImplementedException("Unimplemented propertyType '" + propertyType + "'.");
        }
    }
}
