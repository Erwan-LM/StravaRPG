namespace StravaRpg.Api.Services;

public sealed class QueryStringBuilder
{
    private readonly List<string> values = [];

    public QueryStringBuilder Add(string key, string value)
    {
        values.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
        return this;
    }

    public override string ToString()
    {
        return string.Join("&", values);
    }
}
