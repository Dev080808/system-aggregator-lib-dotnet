using System.Globalization;
using System.Reflection;
using System.Text;

namespace SystemAggregator.Core.Extensions
{
    /// <summary>
    /// A class contains additional features for a query of URI.
    /// </summary>
    public static class UriQueryExtensions
    {
        /// <summary>
        /// Transform the format of a query from an object to a string.
        /// </summary>
        /// <param name="query">An object format of a query.</param>
        /// <returns>A string format of a query.</returns>
        public static string ToQueryString(this object query)
        {
            var parameters = string.Join("&", query.ToQueryParameters(string.Empty));

            return string.IsNullOrEmpty(parameters) ? string.Empty : $"?{parameters}";
        }

        private static IEnumerable<string> ToQueryParameters(this object? value, string prefix)
        {
            if (value == null)
            {
                return Enumerable.Empty<string>();
            }

            return value
                .GetType()
                .GetProperties()
                .SelectMany(property => ToQueryParameters(value, property, prefix))
                .Where(x => x != null);
        }

        private static IEnumerable<string> ToQueryParameters(object value, PropertyInfo property, string prefix)
        {
            var name = prefix + property.Name.ToLower();

            var propertyValue = property.GetValue(value);

            if (propertyValue == null)
            {
                yield break;
            }

            if (property.PropertyType.IsConstructedGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                foreach (var parameter in ListToQueryParameters(name, property, propertyValue))
                {
                    yield return parameter;
                }
            }
            else if (property.PropertyType.IsClass &&
                     property.DeclaringType?.Assembly == property.PropertyType.Assembly)
            {
                foreach (var parameter in ToQueryParameters(propertyValue, $"{name}."))
                {
                    yield return parameter;
                }
            }
            else
            {
                yield return ToQueryParameter(name, propertyValue, property.PropertyType);
            }
        }

        private static IEnumerable<string> ListToQueryParameters(string name, PropertyInfo property, object propertyValue)
        {
            var listToQueryParameters = typeof(UriQueryExtensions)
                .GetMethod(
                    nameof(ListToQueryParameters),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(property.PropertyType.GenericTypeArguments);

            if (listToQueryParameters == null)
            {
                throw new ApplicationException("ListToQueryParameters failed");
            }

            var parameters = (IEnumerable<string>?)listToQueryParameters.Invoke(null, new object?[] { name, propertyValue });

            return parameters ?? Enumerable.Empty<string>();
        }

        private static string ToQueryParameter(string name, object value, Type type)
        {
            var parameterValue = ToQueryParameterValue(value, type);

            return $"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(parameterValue)}";
        }

        private static string ToQueryParameterValue(object value, Type type)
        {
            if (type == typeof(DateTime))
            {
                return ((DateTime)value).ToString("s");
            }

            if (type == typeof(decimal))
            {
                return ((decimal)value).ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString() ?? string.Empty;
        }
    }
}
