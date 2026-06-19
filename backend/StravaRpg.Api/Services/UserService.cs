using Microsoft.EntityFrameworkCore;
using StravaRpg.Api.Infrastructure;
using StravaRpg.Api.Models;

namespace StravaRpg.Api.Services;

public sealed class UserService(AppDbContext db)
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User> GetOrCreateGoogleUserAsync(string email, string name)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(existingUser => existingUser.Email == normalizedEmail);

        if (user is not null)
        {
            user.Name = name;
            await db.SaveChangesAsync();
            return user;
        }

        user = new User
        {
            Id = Guid.NewGuid(),
            Email = normalizedEmail,
            Name = name,
            AvatarUrl = $"https://api.dicebear.com/9.x/thumbs/svg?seed={Uri.EscapeDataString(normalizedEmail)}"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }
}
