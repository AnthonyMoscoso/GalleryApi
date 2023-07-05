using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Dbo
{
    public class Album : EntityAudit
    {
        #region Constructor
        public Album() 
        {
            AlbumImage = new HashSet<AlbumImage>();
        }
        #endregion

        #region Parameters
        /// <summary>
        /// Identifier of the album is a guid code
        /// </summary>
        public Guid IdAlbum { get; set; }
        /// <summary>
        /// Name of the Album
        /// </summary>
        public string Name { get; set; } = string.Empty;

        #endregion

        #region RelationShip
        /// <summary>
        ///  relationships with image
        /// </summary>
        public virtual ICollection<AlbumImage> AlbumImage { get; set; }
        #endregion
    }
}
