using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StravaRpg.Api.Models;

namespace StravaRpg.Api.Services;

public sealed class AuthService(
    HttpClient httpClient,
    UserService userService,
    IOptions<GoogleAuthOptions> googleOptions,
    IOptions<JwtOptions> jwtOptions,
    IOptions<AppOptions> appOptions)
{
    private readonly GoogleAuthOptions google = googleOptions.Value;
    private readonly JwtOptions jwt = jwtOptions.Value;
    private readonly AppOptions app = appOptions.Value;

    public string GetGoogleLoginUrl()
    {
        var query = new QueryStringBuilder()
            .Add("client_id", google.ClientId)
            .Add("redirect_uri", $"{app.BackendUrl}/auth/google/callback")
            .Add("response_type", "code")
            .Add("scope", "openid email profile")
            .Add("access_type", "offline")
            .ToString();

        return $"https://accounts.google.com/o/oauth2/v2/auth?{query}";
    }

    public async Task<string> HandleGoogleCallbackAsync(string code)
    {
        var tokenResponse = await ExchangeCodeAsync(code);
        var googleUser = await GetGoogleUserAsync(tokenResponse.AccessToken);
        var user = await userService.GetOrCreateGoogleUserAsync(googleUser.Email, googleUser.Name);
        var token = CreateJwt(user);

        return $"{app.FrontendUrl}/auth/callback?token={Uri.EscapeDataString(token)}";
    }

    private async Task<GoogleTokenResponse> ExchangeCodeAsync(string code)
    {
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = google.ClientId,
            ["client_secret"] = google.ClientSecret,
            ["code"] = code,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = $"{app.BackendUrl}/auth/google/callback"
        });

        var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<GoogleTokenResponse>()
            ?? throw new InvalidOperationException("Google token response is invalid.");
    }

    private async Task<GoogleUserInfo> GetGoogleUserAsync(string accessToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
        request.Headers.Authorization = new("Bearer", accessToken);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<GoogleUserInfo>()
            ?? throw new InvalidOperationException("Google user response is invalid.");
    }

    private string CreateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };
        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
