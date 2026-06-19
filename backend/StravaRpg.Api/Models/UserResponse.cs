namespace StravaRpg.Api.Models;

public sealed record UserResponse(Guid Id, string Email, string Name, string AvatarUrl, int Level, int Xp)
{
    public static UserResponse FromUser(User user)
    {
        return new UserResponse(user.Id, user.Email, user.Name, user.AvatarUrl, user.Level, user.Xp);
    }
}
