using UnityEngine;
using System;
using DoubleDash.CodingTools.ClassExtensions;

namespace DoubleDash.CodingTools
{
    /// <summary>
    /// This class is currently not thread safe.
    /// </summary>
    /// <typeparam name="TypeVariable"></typeparam>

    [System.Serializable]
    public class VariableReference<TypeVariable> : IVariable<TypeVariable>
    {
        #region Classes

        /// <summary>
        /// This class is used for when the UnityObject does not implement 'IVariable<TypeVariable>', and instead it implements 'TypeVariable'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [System.Serializable] public class InterfacelessVariable<T> : IVariable<T>
        {
            public T Value {
                get;
                set;
            }

            public InterfacelessVariable(T value){
                Value = value;
            }
        }

        #region Exceptions

        /// <summary>
        /// This exception is fired when looking for references of references exceeds a maximum allowed depth.
        /// </summary>
        public class MaximumDepthExceededException : Exception
        {
            public MaximumDepthExceededException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// This exception is fired when the unity object does not have a reference 
        /// </summary>
        public class NoReferenceOnUnityObjectException : Exception
        {
            public NoReferenceOnUnityObjectException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// This exception is fired when the reference is a GameObject, but it has no component that can provide the reference.
        /// </summary>
        public class NoComponentOnGameObjectException : Exception
        {
            public NoComponentOnGameObjectException(string message) : base(message)
            {
            }
        }

        #endregion

        #endregion

        #region Public Variables

        [Tooltip("If this value is true, the value will be taken from an Interface on a UnityEngine.Object, such as a ScriptableObject or MonoBehaviour.\n\n " +
                 "This value is also used as the default return for when an error occours.")]
        public bool useReference;

        [Tooltip("Internal value for when not using a reference. Only simple serializable objects that are not classes or interfaces, such as integers, strings, Vectors or Curves can be internal to this instance.")]
        public TypeVariable internalValue;

        #endregion

        #region Private Variables

        //Reference value that is used when "useReference" is enabled. 
        IVariable<TypeVariable> _referenceValue;

        [Tooltip("Object reference containing the reference value. Whenever this object isn't null, it's used to serialize the reference and obtain the value, or interface to the value.")]
        [SerializeField] UnityEngine.Object objectReference;
        
        static readonly int maximumDepth = 7; //How deep should Cyclical dependency be checked? Failure to identify cyclical dependencies will cause a stack overflow.
        static int currentDepth = 0;          //Current depth search of reference values.

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns true if this Reference is able to get a reference value from the serialized UnityObject. If there's no UnityObject, this also returns false.
        /// </summary>
        public bool HasReferenceValue 
        {
            get 
            {
                //Try to generate the reference value
                try
                {
                    //Get the reference value from the object or cached interface
                    GetReferenceValue(out bool success);

                    //Return the result of the search
                    if (success) return true;
                    else return false;
                }
                catch
                {
                    //If it fails by any error, return false
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtains the reference value of this variable.
        /// </summary>
        public TypeVariable ReferenceValue 
        {
            get 
            {
                //Get the reference value from the object or cached interface
                TypeVariable returnValue = GetReferenceValue(out bool success);

                //If the value was found, return it
                if (success) return returnValue;

                //If the reference wasn't found, throw an error and return the internal value
                Debug.LogError("Trying to get a Reference Value from a '" + this.GetType() + "' of '" + typeof(TypeVariable).GetType().Name + "'. But that Object is null.");
                return internalValue;
            }
            set 
            {
                //Set value to reference.
                _referenceValue.Value = value;
            }
        }

        /// <summary>
        /// Obtains the value contained by this object.
        /// </summary>
        public TypeVariable Value
        {
            get
            {
                if (!useReference && internalValue != null && !(internalValue.GetType().IsClass))
                {
                    return internalValue;
                }
                else
                {
                    return ReferenceValue;
                }
            }
            set
            {
                if (!useReference && internalValue != null)
                {
                    internalValue = value;
                }
                else
                {
                    ReferenceValue = value;
                }
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// This constructor with no parameters is useful for references to types that do not have internal values, such a classes and interfaces. 
        /// </summary>
        public VariableReference()
        {
            this.internalValue = default;
        }

        /// <summary>
        /// Constructor for default internal values. Useful for setting up default ints, floats, strings, etc.
        /// </summary>
        /// <param name="internalValue"></param>
        public VariableReference(TypeVariable internalValue)
        {
            this.internalValue = internalValue;
        }

        #endregion

        #region Private Functions

        TypeVariable GetReferenceValue(out bool success) 
        {
            //If the reference isn't null, get it and stop
            if (_referenceValue != null)
            {
                //If the maximum depth for searching references within references was exceeded, stop.
                if (currentDepth > maximumDepth)
                {
                    string exceptionError = "Maximum depth for finding reference sequences of 'IVariable<" + typeof(TypeVariable).Name + ">' exceeded. " +
                                            "Consider re-structuring your references, or increasing the 'VariableReference's' static 'maximumDepth' value.";

                    throw new MaximumDepthExceededException(exceptionError);
                }

                //Increase the search depth, and try to find a value by triggering another "Get" check.
                currentDepth += 1;
                TypeVariable value = _referenceValue.Value;

                //If it got to this line, then there's no cyclical dependency. If there is a cyclical dependency, the code will loop on the lines above.
                currentDepth = 0;

                //Tag as success and return
                success = true;
                return value;
            }

            //If the reference value is null, then there are no cyclical dependencies that can be incurred from this
            currentDepth = 0;

            //If the reference is null, but the object reference isn't...
            if (objectReference != null)
            {
                //Attempt to extract the reference from the unity object
                _referenceValue = GetVariableFromUnityObject();

                //If successful, tag as success and return the reference
                if (_referenceValue != null)
                {
                    success = true;
                    return _referenceValue.Value;
                }

                //Clear the object
                objectReference = null;
            }

            //Tag as failure and return the default value
            success = false;
            return default;
        }

        IVariable<TypeVariable> GetVariableFromUnityObject()
        {
            if (objectReference != null)
            {
                //Cache the type of the injected object
                Type objectType = objectReference.GetType();

                //If the Object implements the reference to the variable, set that reference and stop.
                if (objectType.ImplementsOrInherits(typeof(IVariable<TypeVariable>))) return (objectReference as IVariable<TypeVariable>);

                //If the Object implements the variable, create a new class to contain the interface and store the value
                if (objectType.ImplementsOrInherits(typeof(TypeVariable))) return new InterfacelessVariable<TypeVariable>((TypeVariable) (object) objectReference);

                //If the Object doesn't implement the IVariable or the Variable, and it is a GameObject...
                if (objectReference is GameObject)
                {
                    //Cache cast as GameObject
                    GameObject objectAsGameObject = objectReference as GameObject;

                    //Attempt to get 'IVariable<TypeVariable>' from a component on that object.
                    IVariable<TypeVariable> gameObjectComponentInterfaceToT = objectAsGameObject.GetComponent<IVariable<TypeVariable>>();

                    //If has found that component, return it
                    if (gameObjectComponentInterfaceToT != null) return gameObjectComponentInterfaceToT;

                    //If type derives from component
                    if (typeof(TypeVariable).ImplementsOrInherits(typeof(Component)) || typeof(TypeVariable).IsInterface)
                    {
                        //Attempt to get 'TypeVariable' from a component on that object.
                        TypeVariable gameObjectComponentT = (TypeVariable)(object)objectAsGameObject.GetComponent<TypeVariable>();

                        //If has found that component, return it
                        if (gameObjectComponentT != null) return new InterfacelessVariable<TypeVariable>(gameObjectComponentT);
                    }

                    //Parse the variable name if possible into declarable types
                    string varName = DoubleDash.CodingTools.DebugTools.TypeParser.ParseCompilerName(typeof(TypeVariable).Name);

                    //If it got this far, throw error for the object reference being a GameObject without the appropriate component.
                    Debug.LogError("The '" + objectType + "' of name '" + objectReference.name + "' does not contain a 'Component' that implements either 'IVariable<" + varName + ">' or '" + varName + "'");

                    return null;
                }
                else 
                {
                    //Parse the variable name if possible into declarable types
                    string varName = DoubleDash.CodingTools.DebugTools.TypeParser.ParseCompilerName(typeof(TypeVariable).Name);

                    //If it got this far, throw error for the object reference not implementing or inheriting from the type, or a reference to the type
                    Debug.LogError("The 'UnityObject.Object' of name '" + objectReference.name + "' of type '" + objectType + "' is not of class '" + varName + "' or implements 'IVariable<" + varName + ">'.");

                    return null;
                }
            }
            //There's no object to attempt to get the IVariable<TypeVariable> from
            return null;
        }

        #endregion

        #region PropertyDrawer Properties

        /// <summary>
        /// This Property is used for PropertyDrawer and accessed via reflection to validate injected UnityObject entries. If it is, it's saved in the _referenceValue.
        /// </summary>
        protected bool IsUnityObjectValid {
            get 
            {
                //Cached variable obtained from object.
                IVariable<TypeVariable> variableFromObject;

                try
                {
                    //Attempt to get the variable from the injected object
                    variableFromObject = GetVariableFromUnityObject();

                    //Check if the type couldn't be obtaned
                    if (variableFromObject == null)
                    {
                        return false;
                    }

                    //Attempt to get that value
                    TypeVariable exceptionTest = variableFromObject.Value;
                
                }
                catch (MaximumDepthExceededException e) 
                {
                    //Print the exception and return false
                    Debug.LogError(e);
                    return false;
                }

                //If the attempt to obtain a variable reference returned null. It failed.
                if (variableFromObject == null) return false;

                //Set the reference value to the one obtained from the object
                _referenceValue = variableFromObject;

                //#if UNITY_EDITOR

                //
                //if(!Application.isPlaying)
                //{
                    //Check if the attempt is being made on the unity editor. If it is, warn that it will cause serialization problems.
                    //Debug.LogWarning("Setting up the Value for a '" + this.GetType().Name + "' of '" + typeof(TypeVariable).GetType().Name + "' directly. This change will not be saved on Play/Stop since it can't be serialized. Instead try to inject the reference via the inspector using the 'Object Reference' field.");
                //}               

                //#endif

                //Return success
                return true;
            }
        }

        /// <summary>
        /// This Property is used for PropertyDrawer and accessed via reflection to reset the _referenceValue. 
        /// If I can figure out how to call this as a function instead of a property i'll change it.
        /// </summary>
        protected bool ResetReferenceValue
        {
            get
            {
                _referenceValue = null;
                return true;
            }
        }

        #endregion
    }
}