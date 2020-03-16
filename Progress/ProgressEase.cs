using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    public class ProgressEase : IMotionProgressBar
    {
#pragma warning disable 0649
        public float delay = 150f;
        public float duration = 0.2f;
        public float oneUnitValue = 10f;
        public float percentDurationIncrease = 20f;
        public Easing.Type ease = Easing.Type.InQuad;
#pragma warning restore 0649

        public float Delay => delay;
        public float Duration => duration;
        public float OneUnitValue => oneUnitValue;
        public float PercentDurationIncrease => percentDurationIncrease;

        ITween IMotionProgressBar.Tween(float value)
        {
            return Easing.Interpolate(ease, ProgressHelper.CaculateDuration(value, oneUnitValue, percentDurationIncrease, duration));
        }
    }
}