using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Worldreaver.Utility
{
    public static partial class Util
    {
        #region Support

        /// <summary>
        /// safe convert the <paramref name="value"/> parameter in range [<paramref name="oldMin"/> to <paramref name="oldMax"/>] to new value in range [<paramref name="newMin"/> to <paramref name="newMax"/>].
        /// <paramref name="value"/> parameter can not out_of_range [<paramref name="oldMin"/> to <paramref name="oldMax"/>]
        /// <paramref name="oldMax"/> parameter must greater than <paramref name="oldMin"/> parameter
        /// <paramref name="newMax"/> parameter must greater than <paramref name="newMin"/> parameter
        /// </summary>
        /// <param name="oldMin">old min value</param>
        /// <param name="oldMax">old max value</param>
        /// <param name="value">value compare</param>
        /// <param name="newMin">new min value</param>
        /// <param name="newMax">new max value</param>
        /// <returns>new value in range [<paramref name="newMin"/> to <paramref name="newMax"/>]</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">out_of_range</exception>
        public static float ClampRemap(this float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            if (value < oldMin || value > oldMax)
            {
                throw Error.ArgumentOutOfRange($"value out of range [{oldMin}..{oldMax}]");
            }

            if (oldMax <= oldMin)
            {
                throw Error.ArgumentOutOfRange("[oldMin..oldMax] range not correct!");
            }

            if (newMax <= newMin)
            {
                throw Error.ArgumentOutOfRange("[newMin..newMax] range not correct!");
            }

            return Remap(value, oldMin, oldMax, newMin, newMax);
        }

        /// <summary>
        /// convert the <paramref name="value"/> parameter in range [<paramref name="oldMin"/> to <paramref name="oldMax"/>] to new value in range [<paramref name="newMin"/> to <paramref name="newMax"/>].
        /// </summary>
        /// <param name="oldMin">old min value</param>
        /// <param name="oldMax">old max value</param>
        /// <param name="value">value compare</param>
        /// <param name="newMin">new min value</param>
        /// <param name="newMax">new max value</param>
        /// <returns>new value in range [<paramref name="newMin"/> to <paramref name="newMax"/>]</returns>
        public static float Remap(this float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }


        /// <summary>
        /// sub array from <paramref name="source"/> parameter a <paramref name="count"/> elements starting at index <paramref name="start"/> parameter.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="source">source array datas</param>
        /// <param name="start">start index</param>
        /// <param name="count">number sub</param>
        /// <returns>sub array</returns>
        public static T[] Sub<T>(this T[] source, int start, int count)
        {
            var result = new T[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = source[start + i];
            }

            return result;
        }

        #region -- calculate --------------------

        /// <summary>
        /// formatting Big Numbers: The “aa” Notation
        ///
        /// number                alphabet
        /// 1                        1
        /// 1000                     1K
        /// 1000000                  1M
        /// 1000000000               1B
        /// 1000000000000            1T
        /// 1000000000000000         1AA
        ///
        /// source  : https://gram.gs/gramlog/formatting-big-numbers-aa-notation/
        /// </summary>
        /// <param name="number">BigInteger</param>
        /// <returns></returns>
        public static string ToAlphabet(this System.Numerics.BigInteger number)
        {
            var str = number.ToString();
            var len = str.Length;
            if (number.Sign < 0 && len <= 4 || number.Sign > 0 && len <= 3) return str;
            var stringBuilder = new System.Text.StringBuilder();
            var index = 0;
            if (number.Sign < 0)
            {
                stringBuilder.Append('-');
                len--;
                index = 1;
            }

            //{0, ""}, {1, "K"}, {2, "M"}, {3, "B"}, {4, "T"}
            var integerPart = len % 3;
            if (integerPart == 0)
            {
                integerPart = 3;
            }

            integerPart += index;
            for (int i = index; i < integerPart; i++)
            {
                stringBuilder.Append(str[i]);
            }

            if (len > 15)
            {
                var n = (len - 16) / 3;
                var firstChar = (char) (65 + n / 26);
                var secondChar = (char) (65 + n % 26);
                stringBuilder.Append(firstChar);
                stringBuilder.Append(secondChar);
            }
            else if (len > 12) stringBuilder.Append('T');
            else if (len > 9) stringBuilder.Append('B');
            else if (len > 6) stringBuilder.Append('M');
            else if (len > 3) stringBuilder.Append('K');

            return stringBuilder.ToString();
        }

        #endregion------------------------------------------------------------\\

        #endregion

        #region Collection

        /// <summary>
        /// safe add <paramref name="value"/> parameter in to <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">collection</param>
        /// <param name="value">element add to collection</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="value" /> parameter is added success in to <paramref name="collection"/>; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><param name="collection"> parameter is null</param></exception>
        public static bool Add<T>(this IList<T> collection, T value)
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            if (collection.Exists(value)) return false;
            collection.Add(value);
            return true;
        }

        /// <summary>
        /// Indicates whether the specified <paramref name="collection"/> parameter is <see langword="null" /> or an empty array ([]).
        /// </summary>
        /// <param name="collection">array</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="collection" /> parameter is <see langword="null" /> or an empty array ([]); otherwise, <see langword="false" />.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this T[] collection)
        {
            if (collection == null)
            {
                return true;
            }

            return collection.Length == 0;
        }

        /// <summary>
        /// Indicates whether the specified <paramref name="collection"/> parameter is <see langword="null" /> or an empty ICollection (List, IEnumerable,...).
        /// </summary>
        /// <param name="collection">ICollection</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="collection" /> parameter is <see langword="null" /> or an empty collection; otherwise, <see langword="false" />.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }

            return collection.Count == 0;
        }

        /// <summary>
        /// Indicates the relationship between <paramref name="x"/> parameter and <paramref name="y"/> parameter.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="x" /> parameter is == <paramref name="y"/>; otherwise, <see langword="false" />.
        /// </returns>
        public static bool Equals<T>(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        /// Indicates the relationship between <paramref name="x"/> parameter and <paramref name="y"/> parameter.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// 0 if the <paramref name="x" /> parameter == <paramref name="y"/> parameter.
        /// less than 0 if the <paramref name="x" /> parameter less than <paramref name="y"/> parameter.
        /// geater than 0 if the <paramref name="x" /> parameter geater than <paramref name="y"/> parameter.
        /// </returns>
        public static int Compare<T>(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }

        /// <summary>
        /// Indicates whether the <paramref name="value"/> parameter exists in the <paramref name="collection"/>.
        /// (Faster using default Contains)
        /// </summary>
        /// <param name="collection">array</param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <see langword="true" /> if the <paramref name="collection" /> parameter is contains <paramref name="value"/>; otherwise, <see langword="false" />.
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        public static bool Exists<T>(this T[] collection, T value)
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var length = collection.Length;
            if (length == 0)
            {
                return false;
            }

            for (var i = 0; i < length; i++)
            {
                if (Equals(collection[i], value)) return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the <paramref name="value"/> parameter exists in the <paramref name="collection"/>.
        /// (same using Contains, in case Contains faster than for loop iterator)
        /// </summary>
        /// <param name="collection">IList</param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <see langword="true" /> if the <paramref name="collection" /> parameter is contains <paramref name="value"/>; otherwise, <see langword="false" />.
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        public static bool Exists<T>(this IList<T> collection, T value)
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            return collection.Contains(value);
        }

        /// <summary>
        /// Indicates the random value in the <paramref name="collection"/>
        /// if <paramref name="collection"/> is empty return default vaule of T
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Rnd<T>(this T[] collection)
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            return collection.Length == 0 ? default : collection[RandomInstance.This.Next(0, collection.Length)];
        }
        
        /// <summary>
        /// Indicates the random value in the <paramref name="collection"/>
        /// if <paramref name="collection"/> is empty return default vaule of T
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Rnd<T>(this IList<T> collection)
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            return collection.Count == 0 ? default : collection[RandomInstance.This.Next(0, collection.Count)];
        }
        
        /// <summary>
        /// Indicates the largest value in the <paramref name="collection"/> parameter with condition <paramref name="selector"/> parameter
        /// ( faster than linq .Select(_=>_.something).Max() and .Max(_=>_something) and .OrderByDescending(_ => _.something).First())
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="selector">condition to query in collection</param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">expression selector</typeparam>
        /// <returns>T have max value follow expression selector</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        /// <exception cref="InvalidOperationException">sequence empty</exception>
        public static T MaxItem<T, TU>(this T[] collection, Func<T, TU> selector) where TU : IComparable<TU>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var first = true;
            var max = default(T);
            var maxKey = default(TU);
            var length = collection.Length;
            for (var i = 0; i < length; i++)
            {
                if (first)
                {
                    max = collection[i];
                    maxKey = selector(max);
                    first = false;
                }
                else
                {
                    var currentKey = selector(collection[i]);
                    if (currentKey.CompareTo(maxKey) <= 0) continue;
                    maxKey = currentKey;
                    max = collection[i];
                }
            }

            if (first) throw new InvalidOperationException("Sequence is empty.");
            return max;
        }

        /// <summary>
        /// Indicates the largest value in the <paramref name="collection"/> parameter with condition <paramref name="selector"/> parameter
        /// ( faster than linq .Select(_=>_.something).Max() and .Max(_=>_something) and .OrderByDescending(_ => _.something).First())
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="selector">condition to query in collection</param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">expression selector</typeparam>
        /// <returns>T have max value follow expression selector</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        /// <exception cref="InvalidOperationException">sequence empty</exception>
        public static T MaxItem<T, TU>(this IList<T> collection, Func<T, TU> selector) where TU : IComparable<TU>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var first = true;
            var max = default(T);
            var maxKey = default(TU);
            var length = collection.Count;
            for (var i = 0; i < length; i++)
            {
                if (first)
                {
                    max = collection[i];
                    maxKey = selector(max);
                    first = false;
                }
                else
                {
                    var currentKey = selector(collection[i]);
                    if (currentKey.CompareTo(maxKey) <= 0) continue;
                    maxKey = currentKey;
                    max = collection[i];
                }
            }

            if (first) throw new InvalidOperationException("Sequence is empty.");
            return max;
        }

        /// <summary>
        /// Indicates the smallest value in the <paramref name="collection"/> parameter with condition <paramref name="selector"/> parameter
        /// ( faster than linq .Select(_=>_.something).Min() and .Min(_=>_something) and .OrderBy(_ => _.something).First() )
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="selector">condition to query in collection</param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">expression selector</typeparam>
        /// <returns>T have min value follow expression selector</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        /// <exception cref="InvalidOperationException">sequence empty</exception>
        public static T MinItem<T, TU>(this T[] collection, Func<T, TU> selector) where TU : IComparable<TU>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var first = true;
            var min = default(T);
            var minKey = default(TU);
            var length = collection.Length;
            for (var i = 0; i < length; i++)
            {
                if (first)
                {
                    min = collection[i];
                    minKey = selector(min);
                    first = false;
                }
                else
                {
                    var currentKey = selector(collection[i]);
                    if (currentKey.CompareTo(minKey) >= 0) continue;
                    minKey = currentKey;
                    min = collection[i];
                }
            }

            if (first) throw new InvalidOperationException("Sequence is empty.");
            return min;
        }

        /// <summary>
        /// Indicates the smallest value in the <paramref name="collection"/> parameter with condition <paramref name="selector"/> parameter
        /// ( faster than linq .Select(_=>_.something).Min() and .Min(_=>_something) and .OrderBy(_ => _.something).First() )
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="selector">condition to query in collection</param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU">expression selector</typeparam>
        /// <returns>T have min value follow expression selector</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        /// <exception cref="InvalidOperationException">sequence empty</exception>
        public static T MinItem<T, TU>(this IList<T> collection, Func<T, TU> selector) where TU : IComparable<TU>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var first = true;
            var min = default(T);
            var minKey = default(TU);
            var length = collection.Count;
            for (var i = 0; i < length; i++)
            {
                if (first)
                {
                    min = collection[i];
                    minKey = selector(min);
                    first = false;
                }
                else
                {
                    var currentKey = selector(collection[i]);
                    if (currentKey.CompareTo(minKey) >= 0) continue;
                    minKey = currentKey;
                    min = collection[i];
                }
            }

            if (first) throw new InvalidOperationException("Sequence is empty.");
            return min;
        }

        /// <summary>
        ///  Indicates the largest value in the <paramref name="collection"/> parameter with its index
        /// </summary>
        /// <param name="collection">Array</param>
        /// <typeparam name="T">primitive type</typeparam>
        /// <returns>(index, max value)</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        public static (int, T) MaxIndex<T>(this T[] collection) where T : IComparable<T>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var result = collection[0];
            var index = 0;

            for (var i = 1; i < collection.Length; i++)
            {
                if (Compare(result, collection[i]) >= 0) continue;
                index = i;
                result = collection[i];
            }

            return (index, result);
        }

        /// <summary>
        ///  Indicates the largest value in the <paramref name="collection"/> parameter with its index
        /// </summary>
        /// <param name="collection">IList</param>
        /// <typeparam name="T">primitive type</typeparam>
        /// <returns>(index, max value)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static (int, T) MaxIndex<T>(this IList<T> collection) where T : IComparable<T>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var result = collection[0];
            var index = 0;
            for (var i = 1; i < collection.Count; i++)
            {
                if (Compare(result, collection[i]) >= 0) continue;
                index = i;
                result = collection[i];
            }

            return (index, result);
        }

        /// <summary>
        ///  Indicates the smallest value in the <paramref name="collection"/> parameter with its index
        /// </summary>
        /// <param name="collection">Array</param>
        /// <typeparam name="T">primitive type</typeparam>
        /// <returns>(index, min value)</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        public static (int, T) MinIndex<T>(this T[] collection) where T : IComparable<T>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var result = collection[0];
            var index = 0;
            for (var i = 1; i < collection.Length; i++)
            {
                if (Compare(result, collection[i]) <= 0) continue;
                index = i;
                result = collection[i];
            }

            return (index, result);
        }

        /// <summary>
        ///  Indicates the smallest value in the <paramref name="collection"/> parameter with its index
        /// </summary>
        /// <param name="collection">IList</param>
        /// <typeparam name="T">primitive type</typeparam>
        /// <returns>(index, min value)</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> parameter is null</exception>
        public static (int, T) MinIndex<T>(this IList<T> collection) where T : IComparable<T>
        {
            if (collection == null)
            {
                throw Error.ArgumentNull("collection");
            }

            var result = collection[0];
            var index = 0;
            for (var i = 1; i < collection.Count; i++)
            {
                if (Compare(result, collection[i]) <= 0) continue;
                index = i;
                result = collection[i];
            }

            return (index, result);
        }

        /// <summary>
        /// shuffle element in array <paramref name="source"/> parameter
        /// </summary>
        /// <param name="source">array</param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this T[] source)
        {
            var n = source.Length;
            while (n > 1)
            {
                n--;
                var k = RandomInstance.This.Next(n + 1);
                var value = source[k];
                source[k] = source[n];
                source[n] = value;
            }
        }

        /// <summary>
        /// shuffle element in <paramref name="source"/> parameter.
        /// </summary>
        /// <param name="source">IList</param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this IList<T> source)
        {
            var n = source.Count;
            while (n > 1)
            {
                n--;
                var k = RandomInstance.This.Next(n + 1);
                var value = source[k];
                source[k] = source[n];
                source[n] = value;
            }
        }

        /// <summary>
        /// shuffle element in dictionary <paramref name="source"/> parameter.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns>new dictionary (shuffled)</returns>
        public static IDictionary<T1, T2> Shuffle<T1, T2>(this IDictionary<T1, T2> source)
        {
            var keys = source.Keys.ToArray();
            var values = source.Values.ToArray();

            var n = source.Count;
            while (n > 1)
            {
                n--;
                var k = RandomInstance.This.Next(n + 1);
                var keyValue = keys[k];
                keys[k] = keys[n];
                keys[n] = keyValue;

                var value = values[k];
                values[k] = values[n];
                values[n] = value;
            }

            return MakeDictionary(keys, values);
        }

        /// <summary>
        /// Add element <paramref name="value"/> parameter with <paramref name="key"/> parameter in to <paramref name="dictionary"/> and indicate whether additional success or faild
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="key" /> parameter added success in to <paramref name="dictionary"/>; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> parameter is null</exception>
        public static bool Add<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw Error.ArgumentNull("dictionary");
            }

            if (dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            return true;
        }

        /// <summary>
        /// make dictionay from elements has <paramref name="values"/> parameter with <paramref name="keys"/> parameter
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/> parameter is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> parameter is null</exception>
        /// <exception cref="ArgumentException">Size <paramref name="keys"/> and size <paramref name="values"/> diffirent!</exception>
        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this TKey[] keys, TValue[] values)
        {
            if (keys == null)
            {
                throw Error.ArgumentNull("keys");
            }

            if (values == null)
            {
                throw Error.ArgumentNull("values");
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Size keys and size values diffirent!");
            }

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Length; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        /// <summary>
        /// make dictionay from elements has <paramref name="values"/> parameter with <paramref name="keys"/> parameter
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="keys"/> parameter is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> parameter is null</exception>
        /// <exception cref="ArgumentException">Size <paramref name="keys"/> and size <paramref name="values"/> diffirent!</exception>
        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this IList<TKey> keys, IList<TValue> values)
        {
            if (keys == null)
            {
                throw Error.ArgumentNull("keys");
            }

            if (values == null)
            {
                throw Error.ArgumentNull("values");
            }

            if (keys.Count != values.Count)
            {
                throw new ArgumentException("Size keys and size values diffirent!");
            }

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Convert <paramref name="position"/> parameter in world to position in canvas <paramref name="canvasRectTransform"/> parameter.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="canvasRectTransform"></param>
        /// <returns>Vector2</returns>
        public static Vector2 ToCanvasPosition(this Vector3 position, RectTransform canvasRectTransform)
        {
            if (Camera.main == null)
            {
                return Vector2.zero;
            }

            var viewport = Camera.main.WorldToViewportPoint(position);
            var sizeDelta = canvasRectTransform.sizeDelta;
            return new Vector3(viewport.x * sizeDelta.x - sizeDelta.x / 2, viewport.y * sizeDelta.y - sizeDelta.y / 2, viewport.z);
        }

        /// <summary>
        /// Convert <paramref name="position"/> parameter in canvas <paramref name="canvasRectTransform"/> parameter to position in world
        /// </summary>
        /// <param name="position"></param>
        /// <param name="canvasRectTransform"></param>
        /// <returns></returns>
        public static Vector2 ToWorldPosition(this Vector3 position, RectTransform canvasRectTransform)
        {
            if (Camera.main == null)
            {
                return Vector2.zero;
            }

            var sizeDelta = canvasRectTransform.sizeDelta;
            return Camera.main.ViewportToWorldPoint(new Vector3((position.x + sizeDelta.x / 2) / sizeDelta.x, (position.y + sizeDelta.y / 2) / sizeDelta.y));
        }

        public static Vector2 ConvertRectTransform(this RectTransform target)
        {
            var rect = target.rect;
            var pivot = target.pivot;
            var fromPivotDerivedOffset = new Vector2(rect.width * pivot.x + rect.xMin, rect.height * pivot.y + rect.yMin);
            return RectTransformUtility.WorldToScreenPoint(null, target.position) + fromPivotDerivedOffset;
        }

        /// <summary>
        /// Converts the anchoredPosition of the <paramref name="from"/> to the <paramref name="to"/>,
        /// taking into consideration offset, anchors and pivot, and returns the new anchoredPosition
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>to.anchoredPosition + localPoint - pivotDerivedOffset</returns>
        public static Vector2 SwitchToRectTransform(this RectTransform from, RectTransform to)
        {
            var screenP = from.ConvertRectTransform();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out var localPoint);
            var rect = to.rect;
            var pivot = to.pivot;
            var pivotDerivedOffset = new Vector2(rect.width * pivot.x + rect.xMin, rect.height * pivot.y + rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        /// <summary>
        /// Converts the anchoredPosition of the <paramref name="from"/> to the <paramref name="to"/>,
        /// taking into consideration offset, anchors and pivot, and returns the new anchoredPosition
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>'localPoint - pivotDerivedOffset'</returns>
        public static Vector2 SwitchToRectTransform2(this RectTransform from, RectTransform to)
        {
            var screenP = from.ConvertRectTransform();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out var localPoint);
            var rect = to.rect;
            var pivot = to.pivot;
            var pivotDerivedOffset = new Vector2(rect.width * pivot.x + rect.xMin, rect.height * pivot.y + rect.yMin);
            return localPoint - pivotDerivedOffset;
        }

        /// <summary>
        /// set <paramref name="pivot"/> parameter for <paramref name="rectTransform"/>
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="pivot"></param>
        public static void SetPivot(RectTransform rectTransform, Vector2 pivot)
        {
            if (rectTransform == null) return;

            var size = rectTransform.rect.size;
            var deltaPivot = rectTransform.pivot - pivot;
            var deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }

        #endregion

        #region Others

        /// <summary>
        /// Indicates file with <paramref name="name"/> file has exist in <paramref name="path"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="path" /> parameter is contains file with <paramref name="name"/>; otherwise, <see langword="false" />.
        /// </returns>
        public static bool ExistsFile(string path, string name)
        {
            return System.IO.File.Exists($"{path}/{name}.wr");
        }

        /// <summary>
        /// remove file with <paramref name="name"/> follow <paramref name="path"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void RemoveFile(string path, string name)
        {
            if (ExistsFile(path, name))
            {
                System.IO.File.Delete($"{path}/{name}.wr");
            }
        }

        /// <summary>
        /// copy <paramref name="data"/> parameter data in to clipboard
        /// </summary>
        /// <param name="data">string</param>
        public static void CopyToClipboard(this string data)
        {
            var textEditor = new TextEditor {text = data};
            textEditor.SelectAll();
            textEditor.Copy();
        }

        #endregion
    }

    public static class Error
    {
        public static Exception ArgumentNull(string argumentName, string message = "")
        {
            return new ArgumentNullException(argumentName, message);
        }

        public static Exception ArgumentOutOfRange(string argumentName)
        {
            return new ArgumentOutOfRangeException(argumentName);
        }

        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element");
        }

        public static Exception MoreThanOneMatch()
        {
            return new InvalidOperationException("Sequence contains more than one matching element");
        }

        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements");
        }

        public static Exception NoMatch()
        {
            return new InvalidOperationException("Sequence contains no matching element");
        }

        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }
    }
}