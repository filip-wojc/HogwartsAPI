namespace HogwartsAPI.Dtos.FileDtos
{
    public class FileDto
    {
        public FileDto(byte[] fileContent, string fileName, string contentType)
        {
            FileContent = fileContent;
            FileName = fileName;
            ContentType = contentType;
        }
        public byte[]? FileContent {  get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
