using System;

namespace ExtraUniRx.Filters
{
    public interface IFilter<T>
    {
        IObservable<T> InputFilter(IObservable<T> inputStream);
        IObservable<T> OutputFilter(IObservable<T> outputStream);
    }
}