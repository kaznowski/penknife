using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.ClassExtensions
{
    public static class ExtensionsList
    {
        private static System.Random rng = new System.Random();

        /// <summary>
        /// Randomizes the order of elements in a list using the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
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
    }
}