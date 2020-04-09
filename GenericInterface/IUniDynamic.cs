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
        bool IsMotion { get; set; }

        /// <summary>
        /// if flag IsMotion is true
        /// flag AllowDisableMotion is effective
        /// if flag AllowDisableMotion is true using another motion for statte disable
        /// </summary>
        bool AllowMotionDisable { get; }

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

        /// <summary>
        /// motion type
        /// </summary>
        EUIMotionType MotionTypeDisable { get; }

        /// <summary>
        /// normal motion
        /// </summary>
        IMotion Motion { get; }

        /// <summary>
        /// motion when button is disable
        /// </summary>
        IMotion MotionDisable { get; }
    }
}