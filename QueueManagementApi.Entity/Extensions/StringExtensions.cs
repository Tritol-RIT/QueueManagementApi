namespace QueueManagementApi.Core.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static int AsInt(this string? value)
    {
        if (int.TryParse(value, out var result))
            return result;

        throw new ArgumentException("Parameter is not a number", nameof(value));
    }
}