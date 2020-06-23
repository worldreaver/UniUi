namespace Worldreaver.UniUI
{
    using UnityEngine;
    using TMPro;
    using System;
    using System.Collections;
    using UniRx;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextCounter : MonoBehaviour
    {
        [SerializeField] private string format = "{0}";
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float duration = 1f;
        [SerializeField] private int fps = 16;

        public TextMeshProUGUI Text => text;
        public int Value { get; private set; }
        private int _currentCache;
        private IDisposable _disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Initialize(
            int value)
        {
            Value = value;
            _currentCache = value;
            text.text = string.Format(format, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextValue"></param>
        /// <returns></returns>
        private IEnumerator IeCountNextValue(
            int nextValue)
        {
            void RefreshEnd()
            {
                Value = nextValue;
                text.text = string.Format(format, Value);
            }

            var isAdd = nextValue >= Value;
            var distance = nextValue - Value;
            var number = (int) (fps * duration);
            var p = distance / number;
            var tick = duration / fps;
            if (p == 0) p = 1;

            for (var i = 0; i < number; i++)
            {
                Value += p;

                text.text = string.Format(format, Value);
                yield return new WaitForSeconds(tick);

                if (isAdd)
                {
                    if (Value < nextValue) continue;
                    RefreshEnd();
                    yield break;
                }

                if (Value > nextValue) continue;
                RefreshEnd();
                yield break;
            }

            RefreshEnd();
        }

        /// <summary>
        /// jump to next value
        /// </summary>
        /// <param name="value"></param>
        public void Jump(
            int value)
        {
            _disposable?.Dispose();
            if (_currentCache != value)
            {
                Value = _currentCache;
                _currentCache = value;
            }

            _disposable = Observable.FromCoroutine(() => IeCountNextValue(value))
                .Subscribe()
                .AddTo(this);
        }

        /// <summary>
        /// increase <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        public void Increase(
            int value)
        {
            Jump(_currentCache + value);
        }

        /// <summary>
        /// decrease <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        public void Decrease(
            int value)
        {
            Jump(_currentCache - value);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy() { _disposable?.Dispose(); }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}