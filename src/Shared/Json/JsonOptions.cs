using System.Text.Json.Serialization;
using System.Text.Json;

namespace Bridge.Shared.Json
{
    /// <summary>
    /// 공용 Json옵션
    /// </summary>
    public class JsonOptions
    {
        private readonly static JsonSerializerOptions jsonSerializerOptions = new()
        {
            Converters = {
                    new JsonStringEnumConverter(null, false)
                }
        };

        public static JsonSerializerOptions Default => jsonSerializerOptions;
    }
}
