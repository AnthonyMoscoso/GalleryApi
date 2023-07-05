using Core.Utilities.Enums;

namespace Core.Utilities.Constants
{
    public static class FileMIME
    {
        public const string PNG = "image/png";
        public const string JPEG = "image/jpeg";
        public const string JPG = "image/jpg";
        public const string GIF = "image/gif";
        public const string SVG = "image/svg+xml";
        public const string TIFF = "image/tiff";
        public const string TIF = "image/tiff";
        public const string PDF = "application/pdf";
        public const string DOC = "application/msword";
        public const string DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string HTML = "text/html";
        public const string XLS = "application/vnd.ms-excel";
        public const string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string TXT = "text/plain";
        public const string PPT = "application/vnd.ms-powerpoint";
        public const string PPTX = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        public const string MD = "text/markdown";
        public const string JSON = "application/json";
        public const string MP4 = "video/mp4";
        public const string AVI = "video/x-msvideo";
        public const string MOV = "video/quicktime";
        public const string FLV = "video/x-flv";
        public const string M4A = "audio/mp4";
        public const string MP3 = "audio/mpeg";
        public const string WAV = "audio/wav";
        public const string FLAC = "audio/flac";
        public const string IO = "application/octet-stream";
        public const string RAR = "application/x-rar-compressed";


        public static string GetMimeFromFileString(string filename)
        {
            string extension = Path.GetExtension(filename);
            extension = extension.Replace(".", "");
            if (!FileFormat.TryParse(extension, true, out FileFormat result))
            {
                return string.Empty;
            }
            if (!Collections.MimeFiles.TryGetValue(result, out string? value))
            {
                return string.Empty;
            }
            return value;
        }


    }
}
