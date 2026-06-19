namespace StravaRpg.Api.Models;

public sealed class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string AvatarUrl { get; set; }
    public int Level { get; set; } = 1;
    public int Xp { get; set; }
}
