/*******************************************************
 * Copyright (C) 2020 worldreaver
 * __________________
 * All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * @author yenmoc phongsoyenmoc.diep@gmail.com
 *******************************************************/

namespace Worldreaver.Timer
{
    public interface ITimeWatch
    {
        /// <summary>
        /// Start timer.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop timer.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resume timer.
        /// </summary>
        void Resume();

        /// <summary>
        /// Pause timer.
        /// </summary>
        void Pause();

        /// <summary>
        /// StopWatch is playing or not.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Current time 
        /// </summary>
        float Time { get; }

        /// <summary>
        /// Get current time sence it starts.
        /// </summary>
        /// <returns></returns>
        System.IObservable<float> TimeAsObservable { get; }

        /// <summary>
        /// Observable stream of IsPlaying flag.
        /// </summary>
        System.IObservable<bool> IsPlayingAsObservable { get; }
    }
}