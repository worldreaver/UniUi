namespace Worldreaver.UniUI
{
    public interface IMotionProgressBar
    {
        /// <summary>
        /// delay run
        /// </summary>
        float Delay { get; }

        /// <summary>
        /// duration
        /// </summary>
        float Duration { get; }

        /// <summary>
        /// one unit value
        /// </summary>
        float OneUnitValue { get; }

        /// <summary>
        /// percent duration increase
        /// </summary>
        float PercentDurationIncrease { get; }

        /// <summary>
        /// tween update value progress
        /// </summary>
        UniTween.ITween Tween(float value);
    }
}