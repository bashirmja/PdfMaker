namespace PdfMaker.Service
{
    public class PdfDto
    {
        public static PdfDto CreateInstance(byte[] fileContent, string fileContentType, string filePath)
        {
            return new PdfDto(fileContent, fileContentType, filePath);
        }

        private PdfDto(byte[] fileContent, string fileContentType, string filePath)
        {
            FileContent = fileContent;
            FileContentType = fileContentType;
            FilePath = filePath;
        }

        public string FilePath { get; private set; }
        public byte[] FileContent { get; private set; }
        public string FileContentType { get; private set; }
    }
}
