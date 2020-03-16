namespace Worldreaver.UniUI
{
    public interface IUniDynamic
    {
        /// <summary>
        /// pivot of RectTransform
        /// </summary>
        EPivot Pivot { get; }

        /// <summary>
        /// is using motion when click button
        /// </summary>
        bool IsMotion { get; }

        /// <summary>
        /// is release only set true when OnPointerUp called
        /// </summary>
        bool IsRelease { get; }

        /// <summary>
        /// make sure OnPointerClick is called on the condition of IsRelease, only set tru when OnPointerExit called
        /// </summary>
        bool IsPrevent { get; }

        /// <summary>
        /// motion type
        /// </summary>
        EUIMotionType MotionType { get; }
    }
}