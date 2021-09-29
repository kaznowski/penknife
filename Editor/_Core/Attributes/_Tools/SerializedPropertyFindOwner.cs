using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System;
using System.Reflection;

namespace DoubleDash.CodingTools.Editor
{
    public static class SerializedPropertyFindOwner
    {

        /// <summary>
        /// Gets the owner of a serialized property.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetSerializedPropertyOwner(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        public static object GetObject(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        public static object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            
            
            var type = source.GetType();
            
            //Get field from the source's type, or any type it derives from
            var f = GetFieldFromHierarchy(type, name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            if (f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        public static FieldInfo GetFieldFromHierarchy(Type type, string name, BindingFlags bindingAttr) 
        {
            //Begin searching from the given type
            var currentType = type;

            //Look up the hierarchy for the field
            while (currentType.BaseType != typeof(object)) 
            {
                //Try to get field
                var field = currentType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                //If was found, return it
                if (field != null) 
                    return field;

                //Otherwise, loop 1 layer up on the hierarchy
                currentType = currentType.BaseType;
            }

            return null;
        } 

        public static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while (index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }
    }
}