using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.Priority
{
    public class PriorityQueue<TypeValue>
        where TypeValue : IPriority
    {
        readonly List<TypeValue> list = new List<TypeValue>();

        //Tag for the list being sorted. When an element is added to a list, the list is no longer sorted.
        public bool ListIsSorted
        {
            get;
            private set;
        }

        /// <summary>
        /// Adds an element to the queue. Complexity depends on whether you want to guarantee that the list is sorted now or whenever it is needed
        /// </summary>
        /// <param name="value"></param>
        /// <param name="checkSorting"></param>
        public void Enqueue(TypeValue value, bool checkSorting = false)
        {
            if (checkSorting)
            {
                if (ListIsSorted) //O(n)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (value.Priority <= list[i].Priority)
                        {
                            list.Insert(i, value);
                        }
                    }
                }
                else //O(N^2)
                {
                    list.Add(value);
                    Sort();
                }
            }
            else //O(1)
            {
                list.Add(value);
                ListIsSorted = false;
            }
        }

        public bool Empty => list.Count <= 0;

        public int Count => list.Count;

        public TypeValue First
        {
            get
            {
                if (Empty)
                {
                    Debug.LogError("Trying to get first element from an empty 'PriorityQueue'.");
                }
                if (!ListIsSorted) Sort();
                return list[0];
            }
        }

        public TypeValue DeQueueFirst()
        {
            if (!ListIsSorted) Sort();
            TypeValue value = First;
            list.RemoveAt(0);
            return value;
        }

        public void Sort()
        {
            list.Sort((TypeValue a, TypeValue b) => b.Priority.CompareTo(a.Priority));
            ListIsSorted = true;
        }
    }
}