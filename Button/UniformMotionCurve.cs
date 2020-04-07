using System;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    /// <summary>
    /// only motion when OnPointerDown call
    /// no motion when OnPointerUp or OnPointerExit
    /// </summary>
    [Serializable]
    public class UniformMotionCurve : IMotion
    {
#pragma warning disable 0649
        public Vector3 percentScaleDown = new Vector3(1.15f, 1.15f, 1f);
        public float durationDown = 0.1f;
        public AnimationCurve curveDown = AnimationCurve.Linear(0, 1, 1, 1);
        public float durationUp = 0.1f;
        public AnimationCurve curveUp = AnimationCurve.Linear(0, 1, 1, 1);
#pragma warning restore 0649

        public Vector3 PercentScaleDown => percentScaleDown;
        private IDisposable _disposableDown;
        private IDisposable _disposableUp;

        public void MotionUp(Vector3 defaultScale,
            RectTransform affectObject)
        {
        }

        public void MotionDown(Vector3 defaultScale,
            RectTransform affectObject)
        {
            DisposeUp();
            var tween = TweenMotion.From(curveDown, durationDown);
            DisposeDown();
            _disposableDown = Tweener.Play(affectObject.localScale, new Vector3(defaultScale.x * PercentScaleDown.x, defaultScale.y * PercentScaleDown.y), tween).DoOnCompleted(() =>
            {
                var tweenUp = TweenMotion.From(curveUp, durationUp);
                DisposeUp();
                _disposableUp = Tweener.Play(affectObject.localScale, defaultScale, tweenUp).SubscribeToLocalScale(affectObject).AddTo(affectObject);
            }).SubscribeToLocalScale(affectObject).AddTo(affectObject);
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