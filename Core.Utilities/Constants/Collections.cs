using Core.Utilities.Enums;

namespace Core.Utilities.Constants
{
    public class Collections
    {
        public static IDictionary<FileFormat, string> MimeFiles = new Dictionary<FileFormat, string>
        {
        { FileFormat.PNG, FileMIME.PNG },
        { FileFormat.JPEG, FileMIME.JPEG },
        { FileFormat.JPG, FileMIME.JPG },
        { FileFormat.GIF, FileMIME.GIF },
        { FileFormat.SVG, FileMIME.SVG },
        { FileFormat.TIFF, FileMIME.TIFF },
        { FileFormat.TIF, FileMIME.TIF },
        { FileFormat.PDF, FileMIME.PDF },
        { FileFormat.DOC, FileMIME.DOC },
        { FileFormat.DOCX, FileMIME.DOCX },
        { FileFormat.HTML, FileMIME.HTML },
        { FileFormat.XLS, FileMIME.XLS },
        { FileFormat.XLSX, FileMIME.XLSX },
        { FileFormat.TXT, FileMIME.TXT },
        { FileFormat.PPT, FileMIME.PPT },
        { FileFormat.PPTX, FileMIME.PPTX },
        { FileFormat.MD, FileMIME.MD },
        { FileFormat.JSON, FileMIME.JSON },
        { FileFormat.MP4, FileMIME.MP4 },
        { FileFormat.AVI, FileMIME.AVI },
        { FileFormat.MOV, FileMIME.MOV },
        { FileFormat.FLV, FileMIME.FLV },
        { FileFormat.M4A, FileMIME.M4A },
        { FileFormat.MP3, FileMIME.MP3 },
        { FileFormat.WAV, FileMIME.WAV },
        { FileFormat.FLAC, FileMIME.FLAC},
        { FileFormat.IO,FileMIME.IO },
        { FileFormat.RAR, FileMIME.RAR } };
    }
}
