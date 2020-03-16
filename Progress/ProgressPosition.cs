using System;
using ExtraUniRx;
using UniRx;
using UnityEngine;
using Worldreaver.UniTween;

namespace Worldreaver.UniUI
{
    public class ProgressPosition : IProgressMode
    {
        public (IDisposable, IDisposable, IDisposable, IDisposable) Initialized(IProgress progress)
        {
            if (progress.ForegroundBar == null) return (null, null, null, null);

            var startPositon = Vector2.zero;
            progress.Current = progress.Max;

            switch (progress.Direction)
            {
                case BarDirections.LeftToRight:
                    progress.Current = progress.Min;
                    startPositon = new Vector2(-progress.ForegroundBar.rect.width, 0);
                    break;
                case BarDirections.DownToUp:
                    progress.Current = progress.Min;
                    startPositon = new Vector2(0, -progress.ForegroundBar.rect.height);
                    break;
            }

            void UpdateValue(Vector2 scale,
                Vector2 pivot)
            {
                progress.ForegroundBar.localScale = scale;
                progress.ForegroundBar.pivot = pivot;

                if (progress.DelayedBar == null) return;
                progress.DelayedBar.localScale = scale;
                progress.DelayedBar.pivot = pivot;
            }

            void UpdateForegroundPosition(float value)
            {
                var normalize = progress.Current / progress.Max;
                var rectValue = progress.ForegroundBar.rect;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (progress.Direction)
                {
                    case BarDirections.DownToUp:
                    case BarDirections.UpToDown:
                        Tweener.Play(progress.ForegroundBar.localPosition.y, -rectValue.height + normalize * rectValue.height, progress.MotionForegroundBar.Tween(value)).SubscribeToLocalPositionY(progress.ForegroundBar).AddTo(progress.ForegroundBar);
                        break;
                    default:
                        Tweener.Play(progress.ForegroundBar.localPosition.x, -rectValue.width + normalize * rectValue.width, progress.MotionForegroundBar.Tween(value)).SubscribeToLocalPositionX(progress.ForegroundBar).AddTo(progress.ForegroundBar);
                        break;
                }
            }

            void UpdateDelayedPosition(float value)
            {
                var normalize = progress.Current / progress.Max;
                var rectValue = progress.ForegroundBar.rect;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (progress.Direction)
                {
                    case BarDirections.DownToUp:
                    case BarDirections.UpToDown:
                        Tweener.Play(progress.DelayedBar.localPosition.y, -rectValue.height + normalize * rectValue.height, progress.MotionDelayedBar.Tween(value)).SubscribeToLocalPositionY(progress.DelayedBar).AddTo(progress.DelayedBar);
                        break;
                    default:
                        Tweener.Play(progress.DelayedBar.localPosition.x, -rectValue.width + normalize * rectValue.width, progress.MotionDelayedBar.Tween(value)).SubscribeToLocalPositionX(progress.DelayedBar).AddTo(progress.DelayedBar);
                        break;
                }
            }

            UpdateValue(Vector2.one, new Vector2(0.5f, 0.5f));
            progress.ForegroundBar.localPosition = startPositon;
            if (progress.ForegroundBar != null)
            {
                progress.ForegroundBar.localPosition = startPositon;
            }

            var foregroundDid = progress.Progress.WhenDid().Throttle(TimeSpan.FromMilliseconds(progress.MotionForegroundBar.Delay)).Subscribe(UpdateForegroundPosition);

            var foregroundDo = progress.Progress.WhenDo().Subscribe(_ =>
            {
                progress.Current += _;
                UpdateForegroundPosition(_);
            });

            IDisposable delayedDid = null;
            IDisposable delayedDo = null;

            if (progress.DelayedBar != null)
            {
                delayedDo = progress.Progress.WhenDo().Throttle(TimeSpan.FromMilliseconds(progress.MotionDelayedBar.Delay)).Subscribe(UpdateDelayedPosition);

                delayedDid = progress.Progress.WhenDid().Subscribe(_ =>
                {
                    progress.Current += _;
                    UpdateDelayedPosition(_);
                });
            }

            return (foregroundDid, foregroundDo, delayedDid, delayedDo);
        }
    }
}