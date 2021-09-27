using System;
using Random = System.Random;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    
    /// <summary>
    /// Generates a random list of indexes ranging from "min" to "max"
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static List<int> GenerateRandomIndexList(int max, int min = 0)
    {
        //Reset/initialize the list
        List<int> list = new List<int>();

        //Generate elements for the list
        for (int i = min; i < max; i++) list.Add(i);

        //Randomize
        list.Shuffle();

        //Return the shuffled index list
        return list;
    }

    public static T RemoveRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

   
    public static IList<T> RemoveList<T>(this IList<T> l, IList<T> otherList)
    {
        if (l == null)
            return null;
        else if (otherList == null)
            return otherList;
        var newList = l.ToList();
        for (int i = 0; i < l.Count; i++)
        {
            var item = l[i];
            if (otherList.Contains(item))
                newList.Remove(item);
        }
        return newList;
    }

    public struct GCFreeEnumerator<T>
    {
        private List<T>.Enumerator enumerator;

        public T Current
        {
            get { return enumerator.Current; }
        }

        public GCFreeEnumerator(List<T> collection)
        {
            enumerator = collection.GetEnumerator();
        }

        public GCFreeEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
    }

    /// <summary>
    /// Use this method to iterate a List in a foreach loop but with no garbage
    /// </summary>
    /// <example>
    /// foreach( var element in myList.Each() )
    /// {
    ///     // code goes here...
    /// }
    /// </example>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static GCFreeEnumerator<T> Each<T>(this List<T> collection)
    {
        return new GCFreeEnumerator<T>(collection);
    }

    /// <summary>
    /// Behaves like System.Linq.Count however it does not generate garbage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int Count<T>(this IList<T> collection, System.Predicate<T> predicate)
    {
        if (predicate == null)
            return 0;

        int count = 0;
        for (int i = 0; i < collection.Count; i++)
        {
            if (predicate(collection[i]))
                count++;
        }

        return count;
    }

    /// <summary>
    /// Behaves like System.Linq.All however it does not generate garbage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool All<T>(this IList<T> collection, System.Predicate<T> predicate)
    {
        if (predicate == null)
            return false;

        for (int i = 0; i < collection.Count; i++)
        {
            if (!predicate(collection[i]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Behaves like System.Linq.Any however it does not generate garbage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool Any<T>(this IList<T> collection, System.Predicate<T> predicate)
    {
        if (predicate == null)
            return false;

        for (int i = 0; i < collection.Count; i++)
        {
            if (predicate(collection[i]))
                return true;
        }

        return false;
    }

    public static bool Any<T>(this IList<T> collection)
    {
        return collection.Count > 0;
    }

    public static bool Any<T, TU>(this Dictionary<T, TU> collection)
    {
        return collection.Count > 0;
    }

    /// <summary>
    /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static T FirstOrDefault<T>(this IList<T> collection)
    {
        return collection.Count > 0 ? collection[0] : default(T);
    }

    /// <summary>
    /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static T FirstOrDefault<T>(this IList<T> collection, System.Predicate<T> predicate)
    {
        for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();)
        {
            if (predicate(enumerator.Current))
                return enumerator.Current;
        }

        return default(T);
    }

    /// <summary>
    /// Pretty format a list as "[ e1, e2, e3, ..., en ]".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static string ToStringFull<T>(this IList<T> predicate)
    {
        if (predicate == null)
            return "null";
        if (predicate.Count <= 0)
            return "[]";

        StringBuilder sb = new StringBuilder();

        sb.Append("[ ");

        for (int i = 0; i < predicate.Count - 1; i++)
        {
            sb.Append(predicate[i]);
            sb.Append(", ");
        }

        sb.Append(predicate[predicate.Count - 1]);
        sb.Append(" ]");

        return sb.ToString();
    }

    public static T Near<T>(this IList<T> self, T reference) where T : Component
    {
        return Near(self, reference, (a, b) => Vector3.Distance(a.transform.position, b.transform.position));
    }

    public static T Near<T>(this IList<T> self, T reference, Func<T, T, float> predicade)
    {
        if (reference == null)
            return self.FirstOrDefault();
        float minDistance = float.MaxValue;
        T bestMatch = default(T);
        foreach (var obj in self)
        {
            var dist = predicade(reference, obj);
            if (dist < minDistance)
            {
                bestMatch = obj;
                minDistance = dist;
            }
        }
        return bestMatch;
    }

}