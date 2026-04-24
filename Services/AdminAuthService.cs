using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Configuration;
using TaskManager.Models;

namespace TaskManager.Services;

public class AdminAuthService
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly IMongoCollection<AdminUser> _admins;
    private readonly PasswordHasher<AdminUser> _passwordHasher = new();

    public AdminAuthService(IMongoClient client, IOptions<MongoSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _admins = database.GetCollection<AdminUser>(settings.Value.AdminUsersCollectionName);
    }

    public async Task EnsureSeedAdminAsync(AdminBootstrapSettings bootstrap)
    {
        var username = bootstrap.Username.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(bootstrap.Password))
        {
            return;
        }

        var normalized = NormalizeUsername(username);
        var existing = await _admins.Find(x => x.NormalizedUsername == normalized).FirstOrDefaultAsync();
        if (existing is not null)
        {
            return;
        }

        var admin = new AdminUser
        {
            Username = username,
            NormalizedUsername = normalized,
            DisplayName = string.IsNullOrWhiteSpace(bootstrap.DisplayName) ? username : bootstrap.DisplayName.Trim(),
            Role = "Admin"
        };

        admin.PasswordHash = _passwordHasher.HashPassword(admin, bootstrap.Password);
        await _admins.InsertOneAsync(admin);
    }

    public async Task<AdminSignInResult> ValidateCredentialsAsync(string username, string password)
    {
        var normalized = NormalizeUsername(username);
        var admin = await _admins.Find(x => x.NormalizedUsername == normalized).FirstOrDefaultAsync();
        if (admin is null)
        {
            return AdminSignInResult.Invalid();
        }

        var now = DateTime.UtcNow;
        if (admin.LockoutEndUtc.HasValue && admin.LockoutEndUtc.Value > now)
        {
            return AdminSignInResult.LockedOut(admin.LockoutEndUtc.Value);
        }

        var verification = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
        if (verification == PasswordVerificationResult.Failed)
        {
            await HandleFailedAttemptAsync(admin, now);
            return AdminSignInResult.Invalid();
        }

        var update = Builders<AdminUser>.Update
            .Set(x => x.FailedLoginAttempts, 0)
            .Set(x => x.LockoutEndUtc, null)
            .Set(x => x.LastLoginUtc, now);

        await _admins.UpdateOneAsync(x => x.Id == admin.Id, update);
        admin.FailedLoginAttempts = 0;
        admin.LockoutEndUtc = null;
        admin.LastLoginUtc = now;

        return AdminSignInResult.Success(admin);
    }

    private async Task HandleFailedAttemptAsync(AdminUser admin, DateTime now)
    {
        var failedAttempts = admin.FailedLoginAttempts + 1;

        UpdateDefinition<AdminUser> update;
        if (failedAttempts >= MaxFailedAttempts)
        {
            update = Builders<AdminUser>.Update
                .Set(x => x.FailedLoginAttempts, 0)
                .Set(x => x.LockoutEndUtc, now.Add(LockoutDuration));
        }
        else
        {
            update = Builders<AdminUser>.Update
                .Set(x => x.FailedLoginAttempts, failedAttempts)
                .Set(x => x.LockoutEndUtc, null);
        }

        await _admins.UpdateOneAsync(x => x.Id == admin.Id, update);
    }

    private static string NormalizeUsername(string username) => username.Trim().ToUpperInvariant();
}

public sealed record AdminSignInResult
{
    public bool IsSuccess { get; init; }

    public bool IsLockedOut { get; init; }

    public DateTime? LockoutEndUtc { get; init; }

    public AdminUser? User { get; init; }

    public static AdminSignInResult Success(AdminUser adminUser) => new()
    {
        IsSuccess = true,
        User = adminUser
    };

    public static AdminSignInResult Invalid() => new();

    public static AdminSignInResult LockedOut(DateTime lockoutEndUtc) => new()
    {
        IsLockedOut = true,
        LockoutEndUtc = lockoutEndUtc
    };
}
