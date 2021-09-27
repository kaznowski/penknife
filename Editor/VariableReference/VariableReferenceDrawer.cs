using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using DoubleDash.CodingTools.ClassExtensions;

namespace DoubleDash.CodingTools.Editor
{
    [CustomPropertyDrawer(typeof(VariableReference<>))]
    public class VariableReferenceDrawer : PropertyDrawer
    {
        //Gets the property that validates if the injected gameobject satisfies the conditions for obtaining the variable.
        private PropertyInfo objectValidatingProperty = null;
        //Gets the property that clears the reference value.
        private PropertyInfo referenceValueClearingProperty = null;

        //Options to display in the popup to select the source of the value.
        private readonly string[] popupOptions = { "Use Local Value", "Use Reference Value" };

        //Cached style to use to draw the popup button.
        private GUIStyle popupStyle;

        //Override the property's ONGUI draw
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Get parent of this property
            object serializedPropertyOwner = SerializedPropertyFindOwner.GetSerializedPropertyOwner(property);

            // Set the Style
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            // Set up the position
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            //Begin looking for changes that will cause updates to the serialized properties.
            EditorGUI.BeginChangeCheck();

            //Get serialized properties
            SerializedProperty useReference = property.FindPropertyRelative("useReference");
            SerializedProperty internalObject = property.FindPropertyRelative("internalValue");
            SerializedProperty objectReference = property.FindPropertyRelative("objectReference");

            //Cache the type that will be used.
            Type fieldType = fieldInfo.FieldType;

            //Extract type if its a list or array, and update the fieldtype
            serializedPropertyOwner = PropertyDrawerTools.ExtractValueFromObject(property, serializedPropertyOwner, ref fieldType, fieldInfo);

            //Get type of T from the internal value
            Type variableType = fieldType.GetField("internalValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FieldType;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            //Store if variable can be drawn
            bool canBeDrawn = CanBeDrawn(variableType);

            //Check if type is serializable
            if (canBeDrawn)
            {
                //Calculate rect space on the inspector for configuration button
                Rect buttonRect = new Rect(position);
                buttonRect.yMin += popupStyle.margin.top;
                buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
                position.xMin = buttonRect.xMax;

                //Check the current value for use reference
                int currentUseReferenceValue = useReference.boolValue ? 1 : 0;

                //Get the editor input for the dropdown.
                currentUseReferenceValue = EditorGUI.Popup(buttonRect, currentUseReferenceValue, popupOptions, popupStyle);

                //Save dropdown value
                useReference.boolValue = currentUseReferenceValue == 1 ? true : false;
            }
            else
            {
                useReference.boolValue = false;
                internalObject = null;
            }

            //If will not use reference, and the type is serializable
            if (useReference.boolValue == false && canBeDrawn)
            {
                //Draw the internal object
                EditorGUI.PropertyField(position, internalObject, GUIContent.none);
                EditorGUI.LabelField(position, new GUIContent("", "Internal value for when not using a reference. Only simple serializable objects that are not classes or interfaces, such as integers, strings, Vectors or Curves can be internal."));
            }
            else
            {
                //Get the value of the old object reference
                UnityEngine.Object previousReference = objectReference.objectReferenceValue;

                //If the type isn't serializable, or using reference, require that it must be an UnityEngine.Object. 
                //We use typeof(UnityEngine.Object) instead of typeof(TypeVariable) so that it can accept GameObjects that implement a TypeVariable or a Reference to TypeVariable.
                objectReference.objectReferenceValue = EditorGUI.ObjectField(position, objectReference.objectReferenceValue, typeof(UnityEngine.Object), true);

                //Parse the name of the variable from the compiler name to a higher level definition. This is to make the inspector tooltip more readable.
                string typeName = DoubleDash.CodingTools.DebugTools.TypeParser.ParseCompilerName(variableType.Name);

                //If the reference was changed, the _referenceValue of the serializedObject must be cleared
                if (previousReference != objectReference.objectReferenceValue)
                {
                    //Get Property from target that resets the reference value used if needed
                    if (referenceValueClearingProperty == null)
                        referenceValueClearingProperty = fieldType.GetProperty("ResetReferenceValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    //Call the property that resets the inspector.
                    referenceValueClearingProperty.GetValue(serializedPropertyOwner);
                }

                //If an object was obtained, it has to be validated
                if (objectReference.objectReferenceValue != null)
                {
                    //Apply the modified propertes to begin validation of the injected object
                    objectReference.serializedObject.ApplyModifiedProperties();

                    //Get Property from target that validates the UnityObject used
                    if (objectValidatingProperty == null)
                        objectValidatingProperty = fieldType.GetProperty("IsUnityObjectValid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    //If the injected object isn't valid.
                    if (!(bool)objectValidatingProperty.GetValue(serializedPropertyOwner))
                    {
                        //Undo current value to the previous value
                        objectReference.objectReferenceValue = previousReference;

                        //Apply the modified propertes after validation
                        objectReference.serializedObject.ApplyModifiedProperties();
                    }
                }

                //Display tooltip
                EditorGUI.LabelField(position, new GUIContent("", "This reference must implement following type: '" + "\n\n" + "IVariable<" + typeName + ">' or '" + typeName + "'"));
            }

            //Set indentation and end property
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Determines whether a type can be drawn as an internal field.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CanBeDrawn(Type type)
        {
            //If its interface or class, it can't be drawn.
            if (type.IsInterface || type.IsClass) return false;

            //if (type.IsInterface) return false;
            //if (type.IsClass && !(type.ImplementsOrInherits(typeof(UnityEngine.Object)))) return true;

            //Check if type is among the simple types of serialization
            bool containsKey = DoubleDash.CodingTools.Editor.PropertyDrawerTools.typeToSerialization.ContainsKey(type);

            //return
            return containsKey;
        }
    }
}