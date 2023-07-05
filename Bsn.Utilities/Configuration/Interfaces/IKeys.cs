using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration.Interfaces
{
    public interface IKeys
    {
        /// <summary>
        /// Gets the Azure-related configuration keys.
        /// </summary>
        IAzureKeys Azure { get; }

        /// <summary>
        /// Gets the JWT-related configuration keys.
        /// </summary>
        IJWTKeys JWT { get; }
    }
}
