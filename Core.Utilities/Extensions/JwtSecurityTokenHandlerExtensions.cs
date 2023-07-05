using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class JwtSecurityTokenHandlerExtensions
    {
        public static bool TryValidateToken(this SecurityTokenHandler tokenHandler, string token, TokenValidationParameters validationParameters, out SecurityToken? validatedToken)
        {
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch
            {
                validatedToken = null;
                return false;
            }
        }
    }
}
