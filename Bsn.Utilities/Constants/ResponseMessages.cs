using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Constants
{
    public static class ResponseMessages
    {
        public const string SuccessDelete = "Successful delete";
        #region Images
        public const string ListOfImages = "List of images";
        public const string ImagesDetail = "Get images detail by ID";
        public const string ImagesRegister = "images registration";
        public const string UpdateImages = "Update images";
        #endregion
        #region Albums
        public const string ListOfAlbums = "List of albums";
        public const string AlbumsDetail = "Get albums detail by ID";
        public const string AlbumsRegister = "albums registration";
        public const string UpdateAlbums = "Update albums";
        #endregion

        #region AlbumImages
        public const string RemoveImageFromAlbum = "remove image from album";
        public const string AddImageFromAlbum = "add image to album";
        #endregion

        public const string GetToken = "Get user token";
        public const string UserToken = "User token";
        public const string Logout = "Log out";

        public const string ListOfGlobalCurrencies = "List of global currencies";
        public const string ListOfPermissions = "List of permissions";

    }
}
