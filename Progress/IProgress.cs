using ExtraUniRx;
using UnityEngine;

namespace Worldreaver.UniUI
{
    public interface IProgress
    {
        TenseSubject<float> Progress { get; }
        float Current { get; set; }
        float Min { get; }
        float Max { get; }
        BarDirections Direction { get; }
        RectTransform ForegroundBar { get; set; }
        RectTransform DelayedBar { get; set; }
        IMotionProgressBar MotionForegroundBar { get; }
        IMotionProgressBar MotionDelayedBar { get; }
    }
}