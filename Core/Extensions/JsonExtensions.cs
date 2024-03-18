using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace SystemAggregator.Core.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions SerializeCamelCaseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private static readonly JsonSerializerOptions DeserializeDefaultOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private static readonly JsonSerializerOptions DeserializeCamelCaseOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public static string ToCamelCaseJson<T>(this T value)
        {
            return JsonSerializer.Serialize(value, SerializeCamelCaseOptions);
        }     
        
        public static JsonElement ToCamelCaseJsonElement<T>(this T value)
        {
            return JsonSerializer.SerializeToElement(value, SerializeCamelCaseOptions);
        }

        public static T? FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, DeserializeDefaultOptions);
        }

        public static T? FromCamelCaseJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, DeserializeCamelCaseOptions);
        }

        public static bool TryFromJson<T>(this JsonElement json, [MaybeNullWhen(false)] out T value)
            where T : class
        {
            value = null;

            try
            {
                value = json.Deserialize<T>(DeserializeDefaultOptions);

                return value != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryFromCamelCaseJson<T>(this JsonElement json, [MaybeNullWhen(false)] out T value)
            where T : class
        {
            value = null;

            try
            {
                value = json.Deserialize<T>(DeserializeCamelCaseOptions);

                return value != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
