using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Enums
{
    public enum FileFormat
    {

        #region image formats
        PNG,
        JPEG,
        JPG,
        GIF,
        SVG,
        TIFF,
        TIF,
        #endregion

        #region documents
        PDF,
        DOC,
        DOCX,
        HTML,
        XLS,
        XLSX,
        TXT,
        PPT,
        PPTX,
        MD,
        JSON,
        #endregion

        #region video 
        MP4,
        AVI,
        MOV,
        FLV,
        #endregion

        #region audio
        M4A,
        MP3,
        WAV,
        FLAC,
        #endregion

        IO,
        RAR
    }


  


}
