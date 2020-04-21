using System;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    [Serializable]
    public class MotionImmediate : IMotion
    {
#pragma warning disable 0649
        public Vector3 percentScaleDown = new Vector3(0.95f, 0.95f, 1f);
#pragma warning restore 0649
        public Vector3 PercentScaleDown => percentScaleDown;
        public IScheduler UnuseScaleScheduler { get; set; } = null;

        public void MotionUp(
            Vector3 defaultScale,
            RectTransform affectObject)
        {
            affectObject.localScale = defaultScale;
        }

        public void MotionDown(
            Vector3 defaultScale,
            RectTransform affectObject)
        {
            affectObject.localScale = new Vector3(defaultScale.x * PercentScaleDown.x, defaultScale.y * PercentScaleDown.y);
        }
    }
}