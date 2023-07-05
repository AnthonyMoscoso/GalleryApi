using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Dbo
{
    public class AlbumImage 
    {
        #region Parameters
        /// <summary>
        /// 
        /// </summary>
        public Guid IdAlbumImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid IdAlbum { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public Guid IdImage { get; set; } 
        #endregion
        #region RelationShip
        /// <summary>
        /// 
        /// </summary>
        public virtual ImageFile? ImageFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Album? Album { get; set; }
        #endregion
    }
}
