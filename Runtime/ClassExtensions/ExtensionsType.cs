using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.ClassExtensions { 
    public static class ExtensionsType
    {
        public static bool ImplementsOrInherits(this Type a, Type b) 
        {
            return (b.IsAssignableFrom(a));
        }
    }
}