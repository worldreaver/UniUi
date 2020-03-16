using System;

namespace ExtraUniRx.Converters
{
    public class TextToDateConverter : IConverter<string, DateTime>, IConverter<DateTime, string>
    {
        private string DateFormat { get; set; }

        public TextToDateConverter(string dateFormat = "d")
        {
            DateFormat = dateFormat;
        }

        public string From(DateTime value)
        { return value.ToString(DateFormat); }

        public DateTime From(string value)
        {
            DateTime.TryParse(value, out var output);
            return output;
        }
    }
}