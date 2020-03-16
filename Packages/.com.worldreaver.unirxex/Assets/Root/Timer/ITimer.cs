using System;
using UniRx;

namespace Worldreaver.Timer
{
    public interface ITimer
    {
        /// <summary>
        /// Start Timer
        /// </summary>
        /// <param name="timeSeconds">Timer's finish time. unit is second.</param>
        void Start(float timeSeconds);

        /// <summary>
        /// Stop Timer
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause Timer
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume Timer
        /// </summary>
        void Resume();

        /// <summary>
        /// Get stream of time (seconds) as observable
        /// </summary>
        IObservable<float> TimeAsObservable { get; }

        /// <summary>
        /// Unit Observable fires when the timer will be started.
        /// </summary>
        IObservable<Unit> StartedAsObservable { get; }

        /// <summary>
        /// Stopped timming as observable
        /// </summary>
        IObservable<float> StoppedTimeAsObservable { get; }

        /// <summary>
        /// Paused timming as observable
        /// </summary>
        IObservable<float> PausedTimeAsObservable { get; }

        /// <summary>
        /// Resumed timming as observable
        /// </summary>
        IObservable<Unit> ResumedAsObservable { get; }

        /// <summary>
        /// Unit Observable fires when the timer will be finished.
        /// </summary>
        IObservable<Unit> FinishedAsObservable { get; }

        /// <summary>
        /// Remain time seconds. It's changing on everyframe.
        /// </summary>
        IObservable<float> RemainTimeAsObservable { get; }

        /// <summary>
        /// Elapsed time seconds. It's changing on everyframe.
        /// </summary>
        IObservable<float> ElapsedTimeAsObservable { get; }

        /// <summary>
        /// Check if it's playing.
        /// </summary>
        IObservable<bool> IsPlayingAsObservable { get; }

        /// <summary>
        /// Return finish time of current timer.
        /// </summary>
        float CurrentFinishTime { get; }

        /// <summary>
        /// Timer is Playing or not
        /// </summary>
        bool IsPlaying { get; }
    }
}