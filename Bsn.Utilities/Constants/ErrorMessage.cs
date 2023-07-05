using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Constants
{
    public static class ErrorMessage
    {
        #region User
        public const string UserNotFound = "User not found";
        public const string UserWithSameNameFound = "User with same name found";
        public const string UserWithSameEmailFound = "User with same email found";
        public const string CannotCreateAdminUser = "You can't create an admin user";
        public const string CannotDeleteAdminUser = "You can't delete an admin user";
        public const string CannotUpdateAdminUser = "You can't update an admin user";
        public const string InvalidUserPasswordFormat = "Invalid user password format";
        public const string InvalidUsernameFormat = "Invalid username format";
        #endregion

        #region AlbumImage
        public const string ImageIsAlreadyInAlbum = "Image is already in this album";
        #endregion

        #region Album
        public const string AlbumNotFound = "album not found";
        #endregion

        #region ImageFile
        public const string ImageFileNotFound = "ImageFile not found";

        #endregion

        #region Login
        public const string IncorrectCredentials = "Incorrect credentials";
        #endregion

        public const string HeaderKeyValueNotCorrect = "The value of the Header key is not correct";
        public const string SystemErrorContactDeveloper = "Error in the system, please contact the developer";

        #region Token
        public const string InvalidToken = "Invalid token";
        public const string EmptyToken = "Token is empty";
        #endregion

        #region Claim
        public const string ClaimValueNotFound = "Claim value not found";
        #endregion

        #region Permission
        public const string PermissionNotFound = "Permission not found";
        #endregion

        #region Miscellaneous
        public const string UnauthorizedBlobClient = "BlobClient must be authorized with Shared Key credentials to create a service SAS";
        public const string TokenRequired = "Token is required";
        public const string InvalidPhoneFormat = "Invalid phone format";
        #endregion

        #region Excel
        public const string WrongExcelFormat = "Wrong format in Excel";

        public const string ForbiddenAccess = "Forbidden Access";
        public const string UnauthorizedAccess = "Unauthorized Access";
        public const string InternalServerError = "Internal Server Error";
        public const string BadRequestError = "Bad request error";
        #endregion

        #region Date
        public const string DateCannotBeNull = "Date cannot be null";
        public const string DateCannotBeEmpty = "Date cannot be empty";
        public const string InvalidDateFormat = "Invalid date format";
        public const string InvalidDateRange = "Invalid date range";
        #endregion

        public const string WrongGuidFormat = "Wrong guid format";

        public const string IdValueIsRequired = "Id value is required";

        public const string NameCannotBeEmpty = "Name cannot be empty";
    }
}
