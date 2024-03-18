using System.Diagnostics.CodeAnalysis;

namespace SystemAggregator.Core.Extensions
{
    public static class CollectionsExtensions
    {
        public static bool IsEmpty<T>([NotNullWhen(false)] this List<T>? list)
        {
            return (list?.Count ?? 0) == 0;
        }

        public static bool IsEmpty<T>([NotNullWhen(false)] this T[]? list)
        {
            return (list?.Length ?? 0) == 0;
        }
    }
}
