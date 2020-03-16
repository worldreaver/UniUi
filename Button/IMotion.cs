namespace Worldreaver.UniUI
{
    public interface IMotion
    {
        /// <summary>
        /// percent of dedault localscale value of button when OnPointerDown
        /// </summary>
        UnityEngine.Vector3 PercentScaleDown { get; }

        /// <summary>
        /// 
        /// </summary>
        void MotionUp(UnityEngine.Vector3 defaultScale,
            UnityEngine.RectTransform affectObject);

        /// <summary>
        /// 
        /// </summary>
        void MotionDown(UnityEngine.Vector3 defaultScale,
            UnityEngine.RectTransform affectObject);
    }
}