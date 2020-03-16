namespace ExtraUniRx.Converters
{
    public class TextToFloatConverter : IConverter<string, float>, IConverter<float, string>
    {
        public string From(float value)
        {
            return $"{value}";
        }

        public float From(string value)
        {
            float.TryParse(value, out var output);
            return output;
        }
    }
}