using System.Collections;

namespace Domain.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool NotEmpty<T>(this T[] array)
    {
        return array is { Length: > 0 };
    }

    public static bool NotEmpty(this ICollection? collection)
    {
        return collection is {Count: > 0};
    }

    public static bool Empty<T>(this T[]? array)
    {
        return array == null || array.Length < 1;
    }

    public static bool Empty(this ICollection? collection)
    {
        return collection == null || collection.Count < 1;
    }
}