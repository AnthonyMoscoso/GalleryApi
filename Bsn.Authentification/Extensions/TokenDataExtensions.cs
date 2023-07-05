using Bsn.Authentification.Models;
using Core.Utilities.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Authentification.Extensions
{
    public static class TokenDataExtensions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(this TokenData token)
        {
            return new(token.Secret.Encoding_UTF8());
        }
        public static SigningCredentials GetSigningCredentials(this TokenData token)
        {
            return new(token.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        }
    }
}
