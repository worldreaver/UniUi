using System;

// ReSharper disable once CheckNamespace
namespace ExtraUniRx
{
    public interface ITenseObserver : IObserver<Tense>
    {
    }

    public interface ITenseObserver<T> : IObserver<Tuple<T, Tense>>
    {
    }
}