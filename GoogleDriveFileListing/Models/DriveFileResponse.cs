namespace GoogleDriveFileListing.Models;

public class DriveFileResponse
{
    public string NextPageToken { get; set; }
    public string Kind { get; set; }
    public bool IncompleteSearch { get; set; }
    public List<FileInfoResponse> Files { get; set; }
}

public class FileInfoResponse
{
    public string Kind { get; set; }
    public string MimeType { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
}
