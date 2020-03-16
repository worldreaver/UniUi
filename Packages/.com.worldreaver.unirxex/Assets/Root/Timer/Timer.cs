/*******************************************************
 * Copyright (C) 2019-2020 worldreaver
 * __________________
 * All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by yenmoc - phongsoyenmoc.diep@gmail.com
 
 *******************************************************/

using System;
using UniRx;

namespace Worldreaver.Timer
{
    public class Timer : ITimer
    {
        #region Property

        private readonly ISubject<Unit> _startedSubject;
        private readonly ISubject<float> _stoppedTimeSubject;
        private readonly ISubject<float> _pausedTimeSubject;
        private readonly ISubject<Unit> _resumedSubject;
        private readonly ITimeWatch _timeWatch;

        public IObservable<float> TimeAsObservable => _timeWatch.TimeAsObservable;
        public IObservable<Unit> StartedAsObservable => _startedSubject;
        public IObservable<float> StoppedTimeAsObservable => _stoppedTimeSubject;
        public IObservable<float> PausedTimeAsObservable => _pausedTimeSubject;
        public IObservable<Unit> ResumedAsObservable => _resumedSubject;
        public IObservable<Unit> FinishedAsObservable => GetFinishedAsObservable();
        public IObservable<float> RemainTimeAsObservable => GetRemainTimeAsObservable();
        public IObservable<float> ElapsedTimeAsObservable => GetElapsedTimeAsObservable();
        public IObservable<bool> IsPlayingAsObservable => _timeWatch.IsPlayingAsObservable; //TimeWatch.TimeAsObservable.Select(time => TimeWatch.IsPlaying && TimeWatch.Time < time).DistinctUntilChanged();
        public float CurrentFinishTime { get; private set; }
        public bool IsPlaying => _timeWatch.IsPlaying && _timeWatch.Time < CurrentFinishTime;

        #endregion

        public Timer() : this(new TimeWatch())
        {
        }

        public Timer(IScheduler scheduler) : this(new TimeWatch(scheduler))
        {
        }

        private Timer(ITimeWatch timeWatch)
        {
            CurrentFinishTime = 0f;
            _startedSubject = new Subject<Unit>();
            _stoppedTimeSubject = new Subject<float>();
            _pausedTimeSubject = new Subject<float>();
            _resumedSubject = new Subject<Unit>();
            _timeWatch = timeWatch;
        }

        public void Start(float timeSeconds)
        {
            _timeWatch.Stop();
            CurrentFinishTime = timeSeconds;
            _timeWatch.Start();
            _startedSubject.OnNext(Unit.Default);
        }

        public void Stop()
        {
            _stoppedTimeSubject.OnNext(_timeWatch.Time);
            _timeWatch.Stop();
        }

        public void Pause()
        {
            _pausedTimeSubject.OnNext(_timeWatch.Time);
            _timeWatch.Pause();
        }

        public void Resume()
        {
            _resumedSubject.OnNext(Unit.Default);
            _timeWatch.Resume();
        }

        private IObservable<Unit> GetFinishedAsObservable()
        {
            return GetElapsedTimeAsObservable().Where(time => time >= CurrentFinishTime).AsUnitObservable().First();
        }

        private IObservable<float> GetRemainTimeAsObservable()
        {
            return _timeWatch.TimeAsObservable.Select(time => CurrentFinishTime - time).Select(time => time < 0 ? 0 : time);
        }

        private IObservable<float> GetElapsedTimeAsObservable()
        {
            return _timeWatch.TimeAsObservable.Select(time => time > CurrentFinishTime ? CurrentFinishTime : time);
        }

        public float GetRemainTime()
        {
            return CurrentFinishTime - _timeWatch.Time;
        }

        public float GetElapsedTime()
        {
            return _timeWatch.Time;
        }

        public void IncreaseTime(float time)
        {
            CurrentFinishTime += time;
        }

        public void DecreaseTime(float time)
        {
            CurrentFinishTime -= time;
        }
    }
}