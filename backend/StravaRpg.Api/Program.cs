using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StravaRpg.Api.Infrastructure;
using StravaRpg.Api.Models;
using StravaRpg.Api.Services;

var builder = WebApplication.CreateBuilder(args);

const string frontendPolicy = "Frontend";

builder.Services.Configure<GoogleAuthOptions>(builder.Configuration.GetSection("Authentication:Google"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Authentication:Jwt"));
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

var jwtOptions = builder.Configuration.GetSection("Authentication:Jwt").Get<JwtOptions>() ?? new JwtOptions();
var jwtKey = Encoding.UTF8.GetBytes(jwtOptions.Secret);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddHttpClient<AuthService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors(frontendPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Text("ok", "text/plain"));
app.MapGet("/auth/google", (AuthService authService) => Results.Redirect(authService.GetGoogleLoginUrl()));
app.MapGet("/auth/google/callback", async (string code, AuthService authService) =>
{
    var redirectUrl = await authService.HandleGoogleCallbackAsync(code);
    return Results.Redirect(redirectUrl);
});
app.MapGet("/auth/me", async (ClaimsPrincipal principal, UserService userService) =>
{
    var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!Guid.TryParse(userId, out var id))
    {
        return Results.Unauthorized();
    }

    var user = await userService.GetByIdAsync(id);

    return user is null ? Results.Unauthorized() : Results.Ok(UserResponse.FromUser(user));
}).RequireAuthorization();

app.Run();
