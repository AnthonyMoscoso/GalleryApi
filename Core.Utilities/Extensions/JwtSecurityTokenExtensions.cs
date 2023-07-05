using Core.Utilities.Ensures;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class JwtSecurityTokenExtensions
    {
        public static TValue? GetClaimValue<TValue>(this JwtSecurityToken token, string key)
        {
            Ensure.That(token, nameof(token)).IsNotNull();
            Ensure.That(key, nameof(key)).NotNullOrEmpty();
            Claim? claim = token.Claims
                     .FirstOrDefault(x => x.Type.ToLower()
                     .Equals(key.ToLower()));
            if (claim == null)
            {
                return default;
            }
            string? value = claim.Value;
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        public static string GetWriteToken(this JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
