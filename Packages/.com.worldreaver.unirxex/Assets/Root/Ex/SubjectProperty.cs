using System;

// ReSharper disable once CheckNamespace
namespace UniRx
{
    /// <summary>
    /// Interface for SubjectProperty act as ISubject and IReactiveProperty
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ISubjectProperty<TValue> : ISubject<TValue>
    {
        TValue Value { get; set; }
        bool HasValue { get; }
    }

    /// <summary>
    /// SubjectProperty is similar to ReactiveProperty.
    ///
    /// Difference:
    /// - Behaviour of IObserver.
    /// - Emit values only if previous value is same.
    ///
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SubjectProperty<TValue> : ISubjectProperty<TValue>
    {
        private TValue _internalValue;

        public TValue Value
        {
            set
            {
                if (!HasValue)
                {
                    HasValue = true;
                }

                _internalValue = value;
                Subject.OnNext(value);
            }
            get => _internalValue;
        }

        public bool HasValue { get; private set; }

        /// <summary>
        /// Set value without any updates. This is for using initialization
        /// </summary>
        public TValue InternalValue
        {
            set => _internalValue = value;
        }

        private ISubject<TValue> Subject { get; } = new Subject<TValue>();

        public void OnCompleted()
        {
            Subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Subject.OnError(error);
        }

        public void OnNext(TValue value)
        {
            Value = value;
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return Subject.Subscribe(observer);
        }
    }
}