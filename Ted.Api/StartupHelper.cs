using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ted.Api
{
    public class StartupHelper
    {
        public const string defaultErrorMessage = "Παρουσιάστηκε σφάλμα κατά την λειτουργία της εφαρμογής, παρακαλούμε επικοινωνήστε με έναν διαχειριστή \n";
        public static PasswordOptions GetPasswordOptions()
        {
            PasswordOptions passwordOptions = new PasswordOptions();
            passwordOptions.RequireDigit = false;
            passwordOptions.RequiredLength = 6;
            passwordOptions.RequireNonAlphanumeric = false;
            passwordOptions.RequireUppercase = false;
            passwordOptions.RequireLowercase = false;
            passwordOptions.RequiredUniqueChars = 4;
            return passwordOptions;
        }

        public static LockoutOptions GetLockoutOptions()
        {
            LockoutOptions lockoutOptions = new LockoutOptions();
            lockoutOptions.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            lockoutOptions.MaxFailedAccessAttempts = 10;
            lockoutOptions.AllowedForNewUsers = true;
            return lockoutOptions;
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration Configuration)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JWTSettings:SecretKey").Value));
            return new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("JWTSettings:Issuer").Value,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("JWTSettings:Audience").Value,

                // Validate the token expiry
                ValidateLifetime = true
            };
        }
    }
}
