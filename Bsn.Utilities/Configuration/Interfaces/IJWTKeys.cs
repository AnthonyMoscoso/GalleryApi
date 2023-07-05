using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration.Interfaces
{
    public interface IJWTKeys
    {
        /// <summary>
        /// Gets the secret used to sign and verify JWT tokens.
        /// </summary>
        string? Secret { get; }

        /// <summary>
        /// Gets the issuer of the JWT token.
        /// </summary>
        string? Issuer { get; }

        /// <summary>
        /// Gets the audience of the JWT token.
        /// </summary>
        string? Audience { get; }
    }
}
