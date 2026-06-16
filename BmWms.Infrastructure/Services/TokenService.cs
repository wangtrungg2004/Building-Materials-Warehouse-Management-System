using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BmWms.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    public TokenService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public (string token, DateTime expiresAt) CreateAccessToken(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new(ClaimTypes.Name,           user.Username),
            new("FullName",                user.FullName)
        };
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key chưa được cấu hình.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.Add(AccessTokenLifetime);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public async Task<string> CreateRefreshTokenAsync(int userID)
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        _context.RefreshTokens.Add(new RefreshToken
        {
            UserID = userID,
            TokenHash = HashToken(rawToken),
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenLifetime),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return rawToken;
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken)) return null;

        var hash = HashToken(rawToken);
        var token = await _context.RefreshTokens
            .Include(t => t.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(t => t.TokenHash == hash);

        if (token is null) return null;

        if (!token.IsActive)
        {
            // Reuse detection: revoke toàn bộ chain nếu token cũ bị tái sử dụng
            if (token.RevokedAt != null && token.ReplacedByTokenHash != null)
                await RevokeDescendantsAsync(token);
            return null;
        }

        return token;
    }

    public async Task RevokeRefreshTokenAsync(RefreshToken token, string? replacedByRawToken = null)
    {
        token.RevokedAt = DateTime.UtcNow;
        if (replacedByRawToken is not null)
            token.ReplacedByTokenHash = HashToken(replacedByRawToken);
        await _context.SaveChangesAsync();
    }

    public string HashToken(string rawToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexString(bytes);
    }

    private async Task RevokeDescendantsAsync(RefreshToken token)
    {
        var current = token;
        while (current.ReplacedByTokenHash is not null)
        {
            var next = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.TokenHash == current.ReplacedByTokenHash);
            if (next is null) break;
            if (next.RevokedAt is null) next.RevokedAt = DateTime.UtcNow;
            current = next;
        }
        await _context.SaveChangesAsync();
    }
}