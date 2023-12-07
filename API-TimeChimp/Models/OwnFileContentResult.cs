namespace Api.Devion.Models
{
    public class OwnFileContentResult
    {
        public byte[]? FileContents { get; set; }
        public string? ContentType { get; set; }
        public string? FileDownloadName { get; set; }
    }
}
