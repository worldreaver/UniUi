using System;
using UniRx;

namespace Worldreaver.Timer
{
    public class TimeWatch : ITimeWatch
    {
        public IObservable<float> TimeAsObservable => TimeProperty;

        public float Time => TimeProperty.Value;

        public bool IsPlaying => _isPlayingProperty.Value;

        public IObservable<bool> IsPlayingAsObservable => _isPlayingProperty;

        private readonly SubjectProperty<bool> _isPlayingProperty = new SubjectProperty<bool>();

        private SubjectProperty<float> TimeProperty { get; } = new SubjectProperty<float>();

        private IObservable<float> OscillatorObservable { get; }

        private IDisposable Subscription { get; set; }
        
        //private static IScheduler DefaultScheduler { get; } = new TimeScheduler();

        /// <summary>
        /// Create TimeWatch by default frame update oscillator
        /// => UnityEngine.Time.deltaTime replace for DefaultScheduler.Delta
        /// </summary>
        public TimeWatch() : this(Observable.EveryUpdate().Select(_ => UnityEngine.Time.deltaTime))
        {
        }
        
        /// <summary>
        /// Create TimeWatch by default frame update oscillator
        /// </summary>
        public TimeWatch(IScheduler scheduler) : this(Observable.EveryUpdate().Select(_ => scheduler.Delta))
        {
        }

        /// <summary>
        /// TimeWatch for counting game time.
        /// </summary>
        /// <param name="oscillatorObservable">oscillator to watch time. it should output the difference of time interval.</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public TimeWatch(IObservable<float> oscillatorObservable)
        {
            OscillatorObservable = oscillatorObservable;
        }

        public void Start()
        {
            if (IsPlaying)
            {
                Stop();
            }

            _isPlayingProperty.Value = true;
            Subscription = OscillatorObservable.StartWith(0).Where(_ => IsPlaying).Scan((sum,
                it) => sum + it).Subscribe(TimeProperty);
        }

        public void Stop()
        {
            _isPlayingProperty.Value = false;
            Subscription?.Dispose();
        }

        public void Resume()
        {
            _isPlayingProperty.Value = true;
        }

        public void Pause()
        {
            _isPlayingProperty.Value = false;
        }
    }

    public class TimeScheduler : IScheduler
    {
        public float Delta => UnityEngine.Time.deltaTime;
    }

    public class UnscaledTimeScheduler : IScheduler
    {
        public float Delta => UnityEngine.Time.unscaledDeltaTime;
    }
}