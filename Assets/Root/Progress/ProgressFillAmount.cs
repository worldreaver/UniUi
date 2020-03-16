using System;
using ExtraUniRx;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    public class ProgressFillAmount : IProgressMode
    {
        public (IDisposable, IDisposable, IDisposable, IDisposable) Initialized(IProgress progress)
        {
            if (progress.ForegroundBar == null) return (null, null, null, null);
            var fillAmount = 1;
            switch (progress.Direction)
            {
                case BarDirections.DownToUp:
                case BarDirections.LeftToRight:
                    progress.Current = progress.Min;
                    fillAmount = 0;
                    break;
            }

            void UpdateValue(Vector3 scale,
                Vector2 pivot)
            {
                progress.ForegroundBar.localScale = scale;
                progress.ForegroundBar.pivot = pivot;
                if (progress.DelayedBar == null) return;
                progress.DelayedBar.localScale = scale;
                progress.DelayedBar.pivot = pivot;
            }

            void UpdateForegroundFillAmount(float value)
            {
                var normalize = progress.Current / progress.Max;
                var foreImage = progress.ForegroundBar.GetComponent<UnityEngine.UI.Image>(); // avoid closure
                Tweener.Play(foreImage.fillAmount, normalize, progress.MotionForegroundBar.Tween(value)).SubscribeToFillAmount(foreImage).AddTo(foreImage);
            }

            void UpdateDelayedFillAmount(float value)
            {
                var normalize = progress.Current / progress.Max;
                var delayImage = progress.DelayedBar.GetComponent<UnityEngine.UI.Image>();
                Tweener.Play(delayImage.fillAmount, normalize, progress.MotionDelayedBar.Tween(value)).SubscribeToFillAmount(delayImage).AddTo(delayImage);
            }

            UpdateValue(Vector3.one, new Vector2(0.5f, 0.5f));

            var foregroundImage = progress.ForegroundBar.GetComponent<UnityEngine.UI.Image>();
            foregroundImage.fillAmount = fillAmount;


            var foregroundDid = progress.Progress.WhenDid().Throttle(TimeSpan.FromMilliseconds(progress.MotionForegroundBar.Delay)).Subscribe(UpdateForegroundFillAmount);

            var foregroundDo = progress.Progress.WhenDo().Subscribe(_ =>
            {
                progress.Current += _;
                UpdateForegroundFillAmount(_);
            });

            IDisposable delayedDid = null;
            IDisposable delayedDo = null;

            if (progress.DelayedBar != null)
            {
                var delayedBarImage = progress.DelayedBar.GetComponent<UnityEngine.UI.Image>();
                delayedBarImage.fillAmount = progress.Current;

                delayedDo = progress.Progress.WhenDo().Throttle(TimeSpan.FromMilliseconds(progress.MotionDelayedBar.Delay)).Subscribe(UpdateDelayedFillAmount);

                delayedDid = progress.Progress.WhenDid().Subscribe(_ =>
                {
                    progress.Current += _;
                    UpdateDelayedFillAmount(_);
                });
            }

            return (foregroundDid, foregroundDo, delayedDid, delayedDo);
        }
    }
}