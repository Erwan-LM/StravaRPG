using System.Text.Json.Serialization;

namespace StravaRpg.Api.Models;

public sealed record GoogleUserInfo(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("name")] string Name
);
