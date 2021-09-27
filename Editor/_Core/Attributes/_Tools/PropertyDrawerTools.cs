using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using DoubleDash.CodingTools.ClassExtensions;
using System.Collections;

namespace DoubleDash.CodingTools.Editor
{
    public static class PropertyDrawerTools
    {
        /// <summary>
        /// Dictionary that given a type shows the kind of property that type is serialized as
        /// </summary>
        public static readonly Dictionary<Type, SerializedPropertyType> typeToSerialization = new Dictionary<Type, SerializedPropertyType>()
        {
            {typeof(int),                SerializedPropertyType.Integer},
            {typeof(bool),               SerializedPropertyType.Boolean},
            {typeof(float),              SerializedPropertyType.Float},
            {typeof(string),             SerializedPropertyType.String},
            {typeof(Color),              SerializedPropertyType.Color},
            {typeof(UnityEngine.Object), SerializedPropertyType.ObjectReference},
            {typeof(LayerMask),          SerializedPropertyType.LayerMask},
            {typeof(Enum),               SerializedPropertyType.Enum},
            {typeof(Vector2),            SerializedPropertyType.Vector2},
            {typeof(Vector3),            SerializedPropertyType.Vector3},
            {typeof(Vector4),            SerializedPropertyType.Vector4},
            {typeof(Rect),               SerializedPropertyType.Rect},
            {typeof(AnimationCurve),     SerializedPropertyType.AnimationCurve},
            {typeof(Bounds),             SerializedPropertyType.Bounds},
        };

        /// <summary>
        /// Gets a type of serialization from an input Type
        /// </summary>
        public static SerializedPropertyType GetSerializedPropertyTypeFromType(Type type)
        {
            //If is interface, change to object field
            if (type.IsInterface) return typeToSerialization[typeof(UnityEngine.Object)];

            //Check if dictionary contains type
            if (!typeToSerialization.ContainsKey(type))
            {
                Debug.LogError("Unimplemented TypeToSerialization in dictionary '" + type + "'.");
                return typeToSerialization[typeof(UnityEngine.Object)];
            }

            //Get from dictionary;
            return typeToSerialization[type];
        }

        /// <summary>
        /// Given a serialized property and object that owns that property, if the object is an array or list, extract the object from the list or array and update its type.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="serializedPropertyOwner"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static object ExtractValueFromObject(SerializedProperty property, object serializedPropertyOwner, ref Type fieldType, FieldInfo fieldInfo)
        {
            //Get type that must be used for getting the internal value
            if (fieldInfo.FieldType.IsArray)
            {
                //Get the argument from the array and find the element.
                fieldType = fieldInfo.FieldType.GetElementType();

                //Return the element within the index
                return (fieldInfo.GetValue(serializedPropertyOwner) as IList)[GetIndexFromArray(property.propertyPath)];
            }
            else if (fieldInfo.FieldType.ImplementsOrInherits(typeof(IList)))
            {
                //Get the argument from the list and find the element.
                fieldType = fieldInfo.FieldType.GetGenericArguments()[0];

                //Return the element within the index
                return (fieldInfo.GetValue(serializedPropertyOwner) as IList)[GetIndexFromArray(property.propertyPath)];
            }
            else
            {
                //Get the object from the serialized parent
                return fieldInfo.GetValue(serializedPropertyOwner);
            }
        }

        /// <summary>
        /// From an inheritance path, get which index from an array that path points towards.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetIndexFromArray(string path)
        {
            var split = path.Split('[');
            var segment = split[split.Length - 1];
            path = segment.Substring(0, segment.Length - 1);

            return int.Parse(path);
        }

        /// <summary>
        /// Draw property that returns the drawers for different types
        /// </summary>
        public static object DrawProperty(Rect position, SerializedPropertyType propertyType, Type type, object value, GUIContent label)
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
                    throw new NotImplementedException("Unimplemented propertyType " + propertyType + ".");
            }
        }
    }
}

