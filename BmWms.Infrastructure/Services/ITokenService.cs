using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmWms.Core.Entities;

namespace BmWms.Infrastructure.Services;

public interface ITokenService
{
    (string token, DateTime expiresAt) CreateAccessToken(User user, IEnumerable<string> roles);
    Task<string> CreateRefreshTokenAsync(int userID);
    Task<RefreshToken?> ValidateRefreshTokenAsync(string rawToken);
    Task RevokeRefreshTokenAsync(RefreshToken token, string? replacedByRawToken = null);
    string HashToken(string rawToken);
}