using System;
using ExtraUniRx;
using UnityEngine;

namespace Worldreaver.UniUI
{
    public enum ProgressMode
    {
        LocalScale,
        FillAmount,
        Position
    }

    public enum BarDirections
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp
    }

    public class UniProgressBar : MonoBehaviour, IProgress
    {
        #region properties

#pragma warning disable 0649
        [SerializeField] private float current;
        [SerializeField] private float max;
        [SerializeField] private BarDirections direction = BarDirections.LeftToRight;
        [SerializeField] private ProgressMode mode = ProgressMode.LocalScale;
        [SerializeField] private RectTransform foregroundBar;
        [SerializeField] private RectTransform delayedBar;
        [SerializeField] private bool curveForegroundBar;
        [SerializeField] private bool curveDelayBar;
        [SerializeField] private bool hasDelayBar;
        private IProgressMode _progressMode;
        [SerializeReference] private IMotionProgressBar _foregroundMotion = new ProgressEase();
        [SerializeReference] private IMotionProgressBar _delaybarMotion = new ProgressEase();

        public TenseSubject<float> Progress { get; private set; }

        public float Current
        {
            get => current;
            set => current = value;
        }

        public float Min { get; private set; }

        public float Max
        {
            get => max;
            private set => max = value;
        }

        public BarDirections Direction => direction;

        public RectTransform ForegroundBar
        {
            get => foregroundBar;
            set => foregroundBar = value;
        }

        public RectTransform DelayedBar
        {
            get => delayedBar;
            set => delayedBar = value;
        }

        public IMotionProgressBar MotionForegroundBar => _foregroundMotion;
        public IMotionProgressBar MotionDelayedBar => _delaybarMotion;

        private bool _isInitialized;
        private IDisposable _foregroundDid;
        private IDisposable _foregroundDo;
        private IDisposable _delayedDid;
        private IDisposable _delayedDo;

#pragma warning restore 0649

        #endregion

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            Invoke(nameof(InitializeProgressMode), 0.1f);
            Invoke(nameof(InitialzieForegroundMotion), 0.15f);
        }
#endif

        #region function

        /// <summary>
        /// Initialize for progress bar
        /// </summary>
        /// <param name="min">value min</param>
        /// <param name="max">value max</param>
        public void Initialized(float min,
            float max)
        {
            if (_isInitialized) return;

            _isInitialized = true;
            Progress = new TenseSubject<float>();
            _foregroundDo?.Dispose();
            _delayedDo?.Dispose();
            _foregroundDid?.Dispose();
            _delayedDid?.Dispose();

            Max = max;
            Min = min;
            Current = max;
            InitializeProgressMode();
            InitialzieForegroundMotion();
            (_foregroundDid, _foregroundDo, _delayedDid, _delayedDo) = _progressMode.Initialized(this);
        }

        /// <summary>
        /// reset progressbar with current min and max
        /// </summary>
        public void ResetProgress()
        {
            _isInitialized = false;
            _progressMode = null;
            Initialized(Min, Max);
        }

        /// <summary>
        /// reset progressbar with new min and max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void ResetProgress(float min,
            float max)
        {
            _isInitialized = false;
            _progressMode = null;
            Initialized(min, max);
        }

        /// <summary>
        /// initialize progress mode
        /// </summary>
        private void InitializeProgressMode()
        {
            if (_progressMode == null)
            {
                switch (mode)
                {
                    case ProgressMode.LocalScale:
                        _progressMode = new ProgressScale();
                        break;
                    case ProgressMode.FillAmount:
                        _progressMode = new ProgressFillAmount();
                        break;
                    case ProgressMode.Position:
                        _progressMode = new ProgressPosition();
                        break;
                }
            }
            else
            {
                switch (mode)
                {
                    case ProgressMode.LocalScale when _progressMode.GetType() != typeof(ProgressScale):
                        _progressMode = new ProgressScale();
                        break;
                    case ProgressMode.FillAmount when _progressMode.GetType() != typeof(ProgressFillAmount):
                        _progressMode = new ProgressFillAmount();
                        break;
                    case ProgressMode.Position when _progressMode.GetType() != typeof(ProgressPosition):
                        _progressMode = new ProgressPosition();
                        break;
                }
            }
        }

        /// <summary>
        /// initialize foreground motion
        /// </summary>
        private void InitialzieForegroundMotion()
        {
            if (curveForegroundBar)
            {
                if (_foregroundMotion == null || _foregroundMotion.GetType() != typeof(ProgressCurve))
                {
                    _foregroundMotion = new ProgressCurve();
                }
            }
            else
            {
                if (_foregroundMotion == null || _foregroundMotion.GetType() != typeof(ProgressEase))
                {
                    _foregroundMotion = new ProgressEase();
                }
            }

            if (hasDelayBar)
            {
                if (curveDelayBar)
                {
                    if (_delaybarMotion == null || _delaybarMotion.GetType() != typeof(ProgressCurve))
                    {
                        _delaybarMotion = new ProgressCurve();
                    }
                }
                else
                {
                    if (_delaybarMotion == null || _delaybarMotion.GetType() != typeof(ProgressEase))
                    {
                        _delaybarMotion = new ProgressEase();
                    }
                }
            }
            else
            {
                _delaybarMotion = null;
            }
        }

        /// <summary>
        /// increase value of progress by value
        /// </summary>
        /// <param name="value">value increase</param>
        public void Increase(float value)
        {
            if (hasDelayBar)
            {
                Progress.Did(value);
                return;
            }

            Progress.Do(value);
        }

        /// <summary>
        /// increase value of progress by value with guard keep value in range [min..max]
        /// </summary>
        /// <param name="value">value increase</param>
        public void IncreaseGuard(float value)
        {
            var valueIncrease = value;
            if (current + value > Max)
            {
                valueIncrease = Max - current;
                if (valueIncrease <= 0f)
                {
                    return;
                }
            }

            if (hasDelayBar)
            {
                Progress.Did(valueIncrease);
                return;
            }

            Progress.Do(valueIncrease);
        }

        /// <summary>
        /// decrease value of progress by value
        /// </summary>
        /// <param name="value">value decrease</param>
        public void Decrease(float value)
        {
            Progress.Do(-value);
        }

        /// <summary>
        /// decrease value of progress by value with guard keep value in range [min..max]
        /// </summary>
        /// <param name="value">value decrease</param>
        public void DecreaseGuard(float value)
        {
            var valueDecrease = value;
            if (current - value < Min)
            {
                valueDecrease = current;
                if (valueDecrease <= 0f)
                {
                    return;
                }
            }

            Progress.Do(-valueDecrease);
        }

        #endregion
    }
}