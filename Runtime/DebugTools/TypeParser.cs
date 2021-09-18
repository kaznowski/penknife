using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.DebugTools
{
    public static class TypeParser
    {
        static Dictionary<string, string> typeParserDictionary = new Dictionary<string, string>
        {
            {"Int32", "int"},
            {"Int64", "int"},
            {"Single", "float"},
        };

        /// <summary>
        /// Gets the name of the variable as used by the compiler and returns a higher definition.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ParseCompilerName(string name)
        {
            if (typeParserDictionary.ContainsKey(name))
            {
                name = typeParserDictionary[name];
            }

            //If it's unable to parse, return the original name
            return name;
        }
    }
}


