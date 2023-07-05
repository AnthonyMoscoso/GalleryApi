using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class EntityAudit
    {
        /// <summary>
        /// User who created this entity
        /// </summary>
        public string IdUserCreater { get; set; } = string.Empty;
        /// <summary>
        /// User who modify this entity
        /// </summary>
        public string? IdUserModifier { get; set; } 
        /// <summary>
        /// Date when entity was created
        /// </summary>
        public DateTimeOffset Created { get; set; }
        /// <summary>
        /// Date when entity was updated
        /// </summary>
        public DateTimeOffset? Updated { get; set; }

    }
}
