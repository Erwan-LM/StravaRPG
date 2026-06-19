namespace StravaRpg.Api.Models;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "StravaRpg";
    public string Audience { get; set; } = "StravaRpg";
    public string Secret { get; set; } = "local-development-secret-change-me-with-at-least-32-characters";
    public int ExpiresMinutes { get; set; } = 10080;
}
