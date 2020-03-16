using System;
using ExtraUniRx.Converters;
using ExtraUniRx.Filters;
using TMPro;
using UniRx;

namespace ExtraUniRx.Bindings
{
    // ReSharper disable once InconsistentNaming
    public static class TMP_TextExtensions
    {
        public static IDisposable BindTextTo(this TMP_Text input, IReadOnlyReactiveProperty<string> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(x => input.text = x, property, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<string> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<int> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, new TextToIntConverter(), BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<float> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, new TextToFloatConverter(), BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<double> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, new TextToDoubleConverter(), BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<DateTime> property, string dateTimeFormat = "d", params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, new TextToDateConverter(dateTimeFormat), BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, IReactiveProperty<TimeSpan> property, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, new TextToTimeSpanConverter(), BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo<T>(this TMP_Text input, IReactiveProperty<T> property, IConverter<string, T> converter, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, property, converter, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo(this TMP_Text input, Func<string> getter, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, getter, null, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindTextTo<T>(this TMP_Text input, Func<T> getter, IConverter<string, T> converter, params IFilter<string>[] filters)
        {
            return GenericBindings.Bind(() => input.text, x => input.text = x, getter, null, converter, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindFontSizeTo(this TMP_Text input, IReactiveProperty<int> property, params IFilter<int>[] filters)
        {
            return GenericBindings.Bind(() => (int) input.fontSize, x => input.fontSize = x, property, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindFontSizeTo(this TMP_Text input, IReadOnlyReactiveProperty<int> property, params IFilter<int>[] filters)
        {
            return GenericBindings.Bind(x => input.fontSize = x, property, filters).AddTo(input);
        }

        public static IDisposable BindFontSizeTo(this TMP_Text input, Func<int> getter, params IFilter<int>[] filters)
        {
            return GenericBindings.Bind(() => (int) input.fontSize, x => input.fontSize = x, getter, null, BindingTypes.OneWay, filters).AddTo(input);
        }

        public static IDisposable BindColorTo(this TMP_Text input, IReactiveProperty<UnityEngine.Color> property, BindingTypes bindingType = BindingTypes.Default, params IFilter<UnityEngine.Color>[] filters)
        {
            return GenericBindings.Bind(() => input.color, x => input.color = x, property, bindingType, filters).AddTo(input);
        }

        public static IDisposable BindColorTo(this TMP_Text input, IReadOnlyReactiveProperty<UnityEngine.Color> property, params IFilter<UnityEngine.Color>[] filters)
        {
            return GenericBindings.Bind(x => input.color = x, property, filters).AddTo(input);
        }

        public static IDisposable BindColorTo(this TMP_Text input, Func<UnityEngine.Color> getter, Action<UnityEngine.Color> setter, BindingTypes bindingType = BindingTypes.Default, params IFilter<UnityEngine.Color>[] filters)
        {
            return GenericBindings.Bind(() => input.color, x => input.color = x, getter, setter, bindingType, filters).AddTo(input);
        }
    }
}