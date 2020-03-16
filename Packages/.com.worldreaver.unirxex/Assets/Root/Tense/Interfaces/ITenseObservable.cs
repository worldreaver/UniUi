using System;

// ReSharper disable once CheckNamespace
namespace ExtraUniRx
{
    public interface ITenseObservable : IObservable<Tense>
    {
    }

    public interface ITenseObservable<T> : IObservable<Tuple<T, Tense>>
    {
    }
}