using System;

// ReSharper disable once CheckNamespace
namespace UniRx.Operators
{
    public class CacheObservable<TValue> : OperatorObservableBase<TValue>, ICachedObservable<TValue>
    {
        private IConnectableObservable<TValue> Source { get; set; }
        private TValue _cachedValue;
        private bool HasSetCacheValue { get; set; }
        public TValue Value => CachedValue;
        private Exception ThrowException { get; set; }
        private bool HasCompleted { get; set; }
        private bool HasSubscribedForCache { get; set; }

        private TValue CachedValue
        {
            get => _cachedValue;
            set
            {
                _cachedValue = value;
                HasSetCacheValue = true;
            }
        }

        public CacheObservable(IObservable<TValue> source) : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            Source = source.Publish();
        }

        protected override IDisposable SubscribeCore(IObserver<TValue> observer, IDisposable cancel)
        {
            observer = new Cache(observer, cancel);
            if (!HasSubscribedForCache)
            {
                Source.Connect();
                Source.Subscribe(_ => CachedValue = _, ex => ThrowException = ex, () =>
                {
                    HasCompleted = true;
                    observer.OnCompleted();
                });
                HasSubscribedForCache = true;
            }

            var disposable = Source.Subscribe(observer.OnNext);
            if (HasSetCacheValue)
            {
                observer.OnNext(CachedValue);
            }

            if (ThrowException != default(Exception))
            {
                observer.OnError(ThrowException);
            }

            if (HasCompleted)
            {
                observer.OnCompleted();
            }

            return disposable;
        }

        private class Cache : OperatorObserverBase<TValue, TValue>
        {
            public Cache(IObserver<TValue> observer, IDisposable cancel) : base(observer, cancel)
            {
            }

            public override void OnNext(TValue value)
            {
                try
                {
                    observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
            {
                try
                {
                    observer.OnError(error);
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnCompleted()
            {
                try
                {
                    observer.OnCompleted();
                }
                finally
                {
                    Dispose();
                }
            }
        }
    }
}
