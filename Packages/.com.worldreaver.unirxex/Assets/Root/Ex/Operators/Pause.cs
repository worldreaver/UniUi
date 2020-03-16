using System;

namespace UniRx.Operators
{
    internal class PauseObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly IObservable<bool> other;
        readonly bool defaultOpen;
        readonly int bufferSize;
        
        public PauseObservable(IObservable<T> source, IObservable<bool> other, bool defaultOpen, int bufferSize) : base(false)
        {
            this.source = source;
            this.other = other;
            this.defaultOpen = defaultOpen;
            this.bufferSize = bufferSize;
        }

        #region Overrides of OperatorObservableBase<T>

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            throw new NotImplementedException();
        }

        #endregion

        class Pause : OperatorObserverBase<T, T>
        {
            public Pause(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
            }

            #region Overrides of OperatorObserverBase<T,T>

            public override void OnNext(T value)
            {
                throw new NotImplementedException();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnCompleted()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
        
    }
}