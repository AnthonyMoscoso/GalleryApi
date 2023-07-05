
using Core.Model;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Auth
{
    public class Credentials : EntityAudit
    {
        #region Parameters
        /// <summary>
        /// Identifier of the credentials
        /// </summary>
        public Guid IdCredentials { get; set; } 
        /// <summary>
        /// User that have this credentials
        /// </summary>
        public Guid IdUser { get; set; }
        /// <summary>
        /// Email of user
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; } = string.Empty;
        #endregion

        #region Relationships
        /// <summary>
        /// User that have this credentials
        /// </summary>
        public virtual User? User { get; set; }
        #endregion
    }
}
