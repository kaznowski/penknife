using UnityEngine;

// Attribute that requires implementation of the provided interface.
public class RequireInterfaceAttribute : PropertyAttribute
{
    // Interface type.
    public System.Type RequiredType { get; private set; }

    /// <summary>
    /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
    /// </summary>
    /// <param name="type">Interface type.</param>
    public RequireInterfaceAttribute(System.Type type)
    {
        this.RequiredType = type;
    }
}