using System;

namespace ExtraUniRx.Converters
{
    public class TextToTimeSpanConverter : IConverter<string, TimeSpan>, IConverter<TimeSpan, string>
    {
        public string From(TimeSpan value)
        {
            return value.ToString();
        }

        public TimeSpan From(string value)
        {
            TimeSpan.TryParse(value, out var output);
            return output;
        }
    }
}