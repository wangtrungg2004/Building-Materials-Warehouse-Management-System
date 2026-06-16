using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmWms.Infrastructure.Services;

public interface IOtpService
{
    Task<string> GenerateAndStoreOtpAsync(string email, TimeSpan validFor);
    Task<bool> ValidateAndConsumeOtpAsync(string email, string otp);
}