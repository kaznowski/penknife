using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.Priority 
{
    public class IndexedPriorityQueue<TypeIndex, TypeValue> : PriorityQueue<TypeValue>
        where TypeValue : IPriority
    {
        public TypeIndex Index
        {
            get;
            set;
        }
    }
}
