using System;
using ExtraUniRx;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    public class ProgressScale : IProgressMode
    {
        public (IDisposable, IDisposable, IDisposable, IDisposable) Initialized(IProgress progress)
        {
            if (progress.ForegroundBar == null) return (null, null, null, null);
            Vector2 tempPivot;
            var temp = Vector3.one;
            switch (progress.Direction)
            {
                case BarDirections.RightToLeft:
                    tempPivot = new Vector2(0f, 0.5f);
                    break;
                case BarDirections.DownToUp:
                    temp = Vector3.right;
                    tempPivot = new Vector2(0.5f, 0f);
                    progress.Current = progress.Min;
                    break;
                case BarDirections.UpToDown:
                    tempPivot = new Vector2(0.5f, 0);
                    break;
                case BarDirections.LeftToRight:
                default:
                    temp = Vector3.up;
                    tempPivot = new Vector2(0f, 0.5f);
                    progress.Current = progress.Min;
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

            void UpdateForegroundLocalScale(float value)
            {
                var normalize = progress.Current / progress.Max;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (progress.Direction)
                {
                    case BarDirections.DownToUp:
                    case BarDirections.UpToDown:
                        Tweener.Play(progress.ForegroundBar.localScale.y, normalize, progress.MotionForegroundBar.Tween(value)).SubscribeToLocalScaleY(progress.ForegroundBar).AddTo(progress.ForegroundBar);
                        break;
                    default:
                        Tweener.Play(progress.ForegroundBar.localScale.x, normalize, progress.MotionForegroundBar.Tween(value)).SubscribeToLocalScaleX(progress.ForegroundBar).AddTo(progress.ForegroundBar);
                        break;
                }
            }

            void UpdateDelayedLocalScale(float value)
            {
                var normalize = progress.Current / progress.Max;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (progress.Direction)
                {
                    case BarDirections.DownToUp:
                    case BarDirections.UpToDown:
                        Tweener.Play(progress.DelayedBar.localScale.y, normalize, progress.MotionForegroundBar.Tween(value)).SubscribeToLocalScaleY(progress.DelayedBar).AddTo(progress.DelayedBar);
                        break;
                    default:
                        Tweener.Play(progress.DelayedBar.localScale.x, normalize, progress.MotionDelayedBar.Tween(value)).SubscribeToLocalScaleX(progress.DelayedBar).AddTo(progress.DelayedBar);
                        break;
                }
            }

            UpdateValue(temp, tempPivot);

            var foregroundDid = progress.Progress.WhenDid().Throttle(TimeSpan.FromMilliseconds(progress.MotionForegroundBar.Delay)).Subscribe(UpdateForegroundLocalScale);

            var foregroundDo = progress.Progress.WhenDo().Subscribe(_ =>
            {
                progress.Current += _;
                UpdateForegroundLocalScale(_);
            });

            IDisposable delayedDid = null;
            IDisposable delayedDo = null;

            if (progress.DelayedBar != null)
            {
                delayedDo = progress.Progress.WhenDo().Throttle(TimeSpan.FromMilliseconds(progress.MotionDelayedBar.Delay)).Subscribe(UpdateDelayedLocalScale);

                delayedDid = progress.Progress.WhenDid().Subscribe(_ =>
                {
                    progress.Current += _;
                    UpdateDelayedLocalScale(_);
                });
            }

            return (foregroundDid, foregroundDo, delayedDid, delayedDo);
        }
    }
}