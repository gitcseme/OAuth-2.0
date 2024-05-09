using GoogleDriveFileListing.Models;

namespace GoogleDriveFileListing.Services;

public class AppConstantDataProvider
{
    public readonly string AuthUrl = "https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount";
    public readonly string TokenUrl = "https://oauth2.googleapis.com/token";
    public readonly string DriveFileUrl = "https://www.googleapis.com/drive/v3/files";
    public readonly string ClientId = "your-client-id";
    public readonly string ClientSecret = "your-client-secret";
    public readonly string RedirectUri = "https://localhost:7220/Home/Callback";
    public readonly string AccessScopes = "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive";

    public void SetTokenResponse(TokenResponse tokenResponse)
    {
        GoogleTokenResponse = tokenResponse;
    }

    public TokenResponse? GoogleTokenResponse { get; set; }
}
