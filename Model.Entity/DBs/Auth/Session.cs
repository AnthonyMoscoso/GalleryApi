using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Auth
{
    public class Session
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public Guid IdUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset ExpirationDate { get; set; }

        #region Relationships
        /// <summary>
        /// 
        /// </summary>
        public virtual User? User { get; set; }
        #endregion
    }
}
