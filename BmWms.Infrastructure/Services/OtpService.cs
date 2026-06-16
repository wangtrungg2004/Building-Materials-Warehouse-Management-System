using Microsoft.EntityFrameworkCore;
using BmWms.Core.Entities;
using BmWms.Infrastructure.Data;
using System.Security.Cryptography;

namespace BmWms.Infrastructure.Services;

public class OtpService : IOtpService
{
    private readonly ApplicationDbContext _context;

    public OtpService(ApplicationDbContext context) => _context = context;

    public async Task<string> GenerateAndStoreOtpAsync(string email, TimeSpan validFor)
    {
        var otp = RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString("D6");
        var oldOtps = _context.PasswordResetOtps.Where(o => o.Email == email);
        _context.PasswordResetOtps.RemoveRange(oldOtps);

        _context.PasswordResetOtps.Add(new PasswordResetOtp
        {
            Email = email,
            Otp = otp,
            ExpiresAt = DateTime.UtcNow.Add(validFor),
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        });

        await _context.SaveChangesAsync();
        return otp;
    }

    public async Task<bool> ValidateAndConsumeOtpAsync(string email, string otp)
    {
        var record = await _context.PasswordResetOtps
            .Where(o => o.Email == email && !o.IsUsed)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (record is null || DateTime.UtcNow > record.ExpiresAt || record.Otp != otp)
            return false;

        record.IsUsed = true;
        await _context.SaveChangesAsync();
        return true;
    }
}