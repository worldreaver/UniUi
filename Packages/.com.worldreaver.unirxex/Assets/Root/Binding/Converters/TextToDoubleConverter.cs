namespace ExtraUniRx.Converters
{
    public class TextToDoubleConverter : IConverter<string, double>, IConverter<double, string>
    {
        public string From(double value)
        {
            return $"{value}";
        }

        public double From(string value)
        {
            double.TryParse(value, out var output);
            return output;
        }
    }
}