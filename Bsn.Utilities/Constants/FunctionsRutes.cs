using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Constants
{
    public static class FunctionsRutes
    {


        #region auth
        public const string auth = "auth";
        public const string logOut = "logOut";
        #endregion

        #region images
        public const string images = "images";
        public const string imagesId = "images/{id}";
        #endregion

        #region albums
        public const string albums = "albums";
        public const string albumsId = "albums/{id}";
        #endregion

        #region albumsImages
        public const string albumImagesAdd = "albums/{idAlbum}/images";
        public const string albumImages = "albums/{idAlbum}/images/{idImage}";
        #endregion

    }
}
