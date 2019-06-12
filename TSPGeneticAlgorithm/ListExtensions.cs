using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions
{
    static class ListExtensions
    {
        /// <summary>
        /// Shuffles a list with Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rand"></param>
        public static void Shuffle<T>(this IList<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
