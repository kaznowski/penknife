using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.ClassExtensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TypeKey,TypeValue> Clone<TypeKey, TypeValue>(this Dictionary<TypeKey, TypeValue> me)
        {
            //Declare dictionary to be returned
            Dictionary<TypeKey, TypeValue> cloneDict = new Dictionary<TypeKey, TypeValue>();

            //Clone each pair
            foreach (KeyValuePair<TypeKey, TypeValue> pair in me) 
            {
                cloneDict.Add(pair.Key, pair.Value);
            }

            //Return the clone
            return cloneDict;
        }
    }
}

