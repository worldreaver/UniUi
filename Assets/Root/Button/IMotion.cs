using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    public interface IMotion
    {
        /// <summary>
        /// percent of dedault localscale value of button when OnPointerDown
        /// </summary>
        UnityEngine.Vector3 PercentScaleDown { get; }

        /// <summary>
        /// motion when up
        /// </summary>
        void MotionUp(UnityEngine.Vector3 defaultScale,
            UnityEngine.RectTransform affectObject);

        /// <summary>
        /// motion when down
        /// </summary>
        void MotionDown(UnityEngine.Vector3 defaultScale,
            UnityEngine.RectTransform affectObject);
        
        /// <summary>
        /// unuse scale scheduler
        /// </summary>
        IScheduler UnuseScaleScheduler { get; }
    }
}