using System;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;
using IScheduler = Worldreaver.UniTween.IScheduler;

namespace Worldreaver.UniUI
{
    /// <summary>
    /// normal motion
    /// </summary>
    [Serializable]
    public class MotionCurve : IMotion
    {
#pragma warning disable 0649
        public Vector3 percentScaleDown = new Vector3(0.92f, 0.92f, 1f);
        public float durationDown = 0.1f;
        public AnimationCurve curveDown = AnimationCurve.Linear(0, 1, 1, 1);
        public float durationUp = 0.1f;
        public AnimationCurve curveUp = AnimationCurve.Linear(0, 1, 1, 1);
#pragma warning restore 0649

        public Vector3 PercentScaleDown => percentScaleDown;
        public IScheduler UnuseScaleScheduler => _scheduler ?? (_scheduler = new UnscaledTimeScheduler());

        private IScheduler _scheduler;
        private IDisposable _disposableDown;
        private IDisposable _disposableUp;

        public void MotionUp(
            Vector3 defaultScale,
            RectTransform affectObject)
        {
            DisposeDown();
            var tween = TweenMotion.From(curveUp, durationUp);
            DisposeUp();
            _disposableUp = Tweener.Play(affectObject.localScale, defaultScale, tween, UnuseScaleScheduler)
                .SubscribeToLocalScale(affectObject)
                .AddTo(affectObject);
        }

        public void MotionDown(
            Vector3 defaultScale,
            RectTransform affectObject)
        {
            DisposeUp();
            var tween = TweenMotion.From(curveDown, durationDown);
            DisposeDown();
            _disposableDown = Tweener.Play(affectObject.localScale,
                    new Vector3(defaultScale.x * PercentScaleDown.x, defaultScale.y * PercentScaleDown.y),
                    tween,
                    UnuseScaleScheduler)
                .SubscribeToLocalScale(affectObject)
                .AddTo(affectObject);
        }

        public void DisposeDown()
        {
            _disposableDown?.Dispose();
            _disposableDown = null;
        }

        public void DisposeUp()
        {
            _disposableUp?.Dispose();
            _disposableUp = null;
        }
    }
}