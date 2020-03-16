using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace UniRx
{
    /// <summary>
    /// EmptyBehaviorSubject is BehaviorSubject which holds nothing on the beginning.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class EmptyBehaviorSubject<TValue> : ISubject<TValue>, IDisposable
    {
        private bool _isDisposed;
        private bool _isFirstEmitted;
        private TValue _value = default(TValue);

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private IList<IObserver<TValue>> _observerList = new List<IObserver<TValue>>();

        public void OnCompleted()
        {
            if (_isDisposed) return;

            foreach (var observer in _observerList)
            {
                observer.OnCompleted();
            }

            Dispose();
        }

        public void OnError(Exception error)
        {
            if (_isDisposed) return;

            foreach (var observer in _observerList)
            {
                observer.OnError(error);
            }

            Dispose();
        }

        public void OnNext(TValue value)
        {
            if (_isDisposed) return;

            _isFirstEmitted = true;
            _value = value;

            foreach (var observer in _observerList)
            {
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (_isDisposed) return new Subscription(this);

            _observerList.Add(observer);

            if (_isFirstEmitted)
            {
                observer.OnNext(_value);
            }

            return new Subscription(this);
        }

        public void Dispose()
        {
            _isDisposed = true;
            _observerList.Clear();
        }

        private class Subscription : IDisposable
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            private EmptyBehaviorSubject<TValue> _subject;

            public Subscription(EmptyBehaviorSubject<TValue> subject)
            {
                _subject = subject;
            }

            public void Dispose()
            {
                _subject.Dispose();
            }
        }
    }
}