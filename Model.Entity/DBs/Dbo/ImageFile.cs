using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBs.Dbo
{
    public class ImageFile : EntityAudit
    {
        #region Constructor
        public ImageFile() 
        {
            AlbumImage = new HashSet<AlbumImage>();
        }
        #endregion

        #region Parameters
        /// <summary>
        /// Identifier of image, is a guid code
        /// </summary>
        public Guid IdImage { get; set; }
        /// <summary>
        /// Name of image file
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description of image file
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Url with image file
        /// </summary>
        public string Url { get; set; } = string.Empty;

        #endregion

        #region RelationShip
        /// <summary>
        ///  relationships with image
        /// </summary>
        public virtual ICollection<AlbumImage> AlbumImage { get; set; }
        #endregion
    }
}
