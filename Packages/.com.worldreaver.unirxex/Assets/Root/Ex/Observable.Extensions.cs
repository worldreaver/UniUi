using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx.Operators;

// ReSharper disable CheckNamespace
namespace UniRx
{
    public static partial class ObservableEx
    {
        public static ICachedObservable<TValue> Cache<TValue>(this IObservable<TValue> source)
        {
            return new CacheObservable<TValue>(source);
        }

        public static ICachedObservable<TValue> CacheAll<TValue>(this IObservable<TValue> source)
        {
            return new CacheAllObservable<TValue>(source);
        }

        public static IObservable<TValue> Returns<TValue>(IEnumerable<TValue> values)
        {
            return Observable.Create<TValue>(observer =>
            {
                foreach (var value in values)
                {
                    observer.OnNext(value);
                }

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public static IObservable<TSource> IsDefault<TSource>(this IObservable<TSource> source)
        {
            return source.Where(_ => Equals(_, default(TSource)));
        }

        public static IObservable<TSource> IsDefault<TSource, TValue>(this IObservable<TSource> source, Func<TSource, TValue> selector)
        {
            return source.Where(_ => Equals(_, default(TSource)) || Equals(selector(_), default(TValue)));
        }

        public static IObservable<TSource> IsNotDefault<TSource>(this IObservable<TSource> source)
        {
            return source.Where(_ => !Equals(_, default(TSource)));
        }

        public static IObservable<TSource> IsNotDefault<TSource, TValue>(this IObservable<TSource> source, Func<TSource, TValue> selector)
        {
            return source.Where(_ => !Equals(_, default(TSource)) && !Equals(selector(_), default(TValue)));
        }

        public static IObservable<T> FromCoroutineWithInitialValue<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, T initialValue)
        {
            return new FromCoroutineWithInitialValueObservable<T>(coroutine, initialValue);
        }

        public static IObservable<T> FromMicroCoroutineWithInitialValue<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, T initialValue, FrameCountType frameCountType = FrameCountType.Update)
        {
            return new FromMicroCoroutineWithInitialValueObservable<T>(coroutine, initialValue, frameCountType);
        }
        
        public static IObservable<float> Range(this IObservable<float> source, float min, float max)
        {
            return source.Select(x => UnityEngine.Mathf.Clamp(x - min, 0.0f, max) / (max - min));
        }

        public static IObservable<T> Loop<T>(this IObservable<T> source)
        {
            return source.Repeat();
        }

        public static IObservable<T> Loop<T>(this IObservable<T> source, int repeatCount)
        {
            return Observable.Range(0, repeatCount).Select(x => source).Concat();
        }
    }
}