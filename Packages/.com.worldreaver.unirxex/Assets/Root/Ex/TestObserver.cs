using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace UniRx
{
    public class TestObserver<TValue> : IObserver<TValue>
    {
        public int OnCompletedCount { get; private set; }

        public int OnNextCount { get; private set; }

        public int OnErrorCount { get; private set; }

        public List<TValue> OnNextValues { get; private set; }

        public List<Exception> OnErrorValues { get; private set; }

        public TValue OnNextLastValue => OnNextValues.Count > 0 ? OnNextValues[OnNextValues.Count - 1] : default(TValue);

        public Exception OnErrorLastValue => OnErrorValues.Count > 0 ? OnErrorValues[OnErrorValues.Count - 1] : default(Exception);

        public TestObserver()
        {
            OnNextValues = new List<TValue>();
            OnErrorValues = new List<Exception>();
        }

        public void OnCompleted()
        {
            OnCompletedCount++;
        }

        public void OnError(Exception error)
        {
            OnErrorCount++;
            OnErrorValues.Add(error);
        }

        public void OnNext(TValue value)
        {
            OnNextCount++;
            OnNextValues.Add(value);
        }
    }
}