using System;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    [Serializable]
    public class MotionCurveEase : IMotion
    {
#pragma warning disable 0649
        public Vector3 percentScaleDown = new Vector3(0.95f, 0.95f, 1f);
        public float durationDown = 0.1f;
        public AnimationCurve curveDown = AnimationCurve.Linear(0, 1, 1, 1);
        public float durationUp = 0.1f;
        public Easing.Type easeUp = Easing.Type.Linear;
#pragma warning restore 0649

        public Vector3 PercentScaleDown => percentScaleDown;
        public IDisposable DisposableDown { get; private set; }
        public IDisposable DisposableUp { get; private set; }

        public void MotionUp(Vector3 defaultScale,
            RectTransform affectObject)
        {
            DisposeDown();
            var tween = Easing.Interpolate(easeUp, durationUp);
            DisposeUp();
            DisposableUp = Tweener.Play(affectObject.localScale, defaultScale, tween).SubscribeToLocalScale(affectObject).AddTo(affectObject);
        }

        public void MotionDown(Vector3 defaultScale,
            RectTransform affectObject)
        {
            DisposeUp();
            var tween = TweenMotion.From(curveDown, durationDown);
            DisposeDown();
            DisposableDown = Tweener.Play(affectObject.localScale, new Vector3(defaultScale.x * PercentScaleDown.x, defaultScale.y * PercentScaleDown.y), tween).SubscribeToLocalScale(affectObject).AddTo(affectObject);
        }

        public void DisposeDown()
        {
            DisposableDown?.Dispose();
            DisposableDown = null;
        }

        public void DisposeUp()
        {
            DisposableUp?.Dispose();
            DisposableUp = null;
        }
    }
}