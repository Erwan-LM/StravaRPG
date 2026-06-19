using System.Text.Json.Serialization;

namespace StravaRpg.Api.Models;

public sealed record GoogleTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken
);
