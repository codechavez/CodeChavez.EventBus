namespace CodeChavez.EventBus.Abstractions.Extensions;

public static class EventBusOptionExtensions
{
    /// <summary>
    /// Converts an IEnumerable of host strings into a single comma-separated string
    /// </summary>
    /// <param name="hosts"></param>
    /// <returns></returns>
    public static string GetString(this IEnumerable<string> hosts)
    {
        return string.Join(",", hosts);
    }
}