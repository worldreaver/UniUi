﻿#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
using System;

#endif
// ReSharper disable once CheckNamespace
namespace UniRx
{
    public interface ICachedObservable<out TValue> : IObservable<TValue>
    {
        TValue Value { get; }
    }
}