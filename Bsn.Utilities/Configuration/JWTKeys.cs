using Bsn.Utilities.Configuration.Interfaces;
using Bsn.Utilities.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration
{
    public class JWTKeys : IJWTKeys
    {
        private readonly IConfiguration _configuration;
        public JWTKeys(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string? Secret => _configuration[AppSettings.jwt_secret_key];
        public string? Issuer => _configuration[AppSettings.jwt_Issuer];
        public string? Audience => _configuration[AppSettings.jwt_audicence];
    }
}
