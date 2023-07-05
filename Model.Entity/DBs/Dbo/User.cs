using Core.Model;
using Model.Entity.DBs.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Dbo
{
    public class User : EntityAudit
    {
        #region Parameters
        /// <summary>
        /// Identifier of the user
        /// </summary>
        public Guid IdUser { get; set; } 
        /// <summary>
        /// Name of user
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public string? UrlImage { get; set; }
        #endregion
        #region Relationships
        /// <summary>
        /// Session of User
        /// </summary>
        public virtual Session? Session { get; set; }
        #endregion

        #region Relationships
        /// <summary>
        /// Credential that have a user
        /// </summary>
        public virtual Credentials? Credentials { get; set; }
        #endregion
    }
}
