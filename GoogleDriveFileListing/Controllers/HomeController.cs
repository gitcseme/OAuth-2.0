using GoogleDriveFileListing.Models;
using GoogleDriveFileListing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoogleDriveFileListing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppConstantDataProvider _dataProvider;

        public HomeController(ILogger<HomeController> logger, AppConstantDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            string concentUrl = new EncodedUrlBuilder(_dataProvider.AuthUrl)
                .AddParam("redirect_uri", _dataProvider.RedirectUri)
                .AddParam("prompt", "consent")
                .AddParam("response_type", "code")
                .AddParam("client_id", _dataProvider.ClientId)
                .AddParam("scope", _dataProvider.AccessScopes)
                .AddParam("access_type", "offline")
                .AddParam("service", "lso")
                .AddParam("o2v", "2")
                .AddParam("theme", "mn")
                .AddParam("ddm", "0")
                .AddParam("flowName", "GeneralOAuthFlow")
                .Build();

            return View(new OAuthViewModel { ConcentUrl = concentUrl });
        }

        public async Task<IActionResult> Callback(string code)
        {
            var tokenRequest = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("client_id", _dataProvider.ClientId),
                new KeyValuePair<string, string>("client_secret", _dataProvider.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", _dataProvider.RedirectUri),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            ]);

            using var httpClient = new HttpClient();
            var tokenResponse = await httpClient.PostAsync(_dataProvider.TokenUrl, tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();

            var responseContent = await tokenResponse.Content.ReadAsStringAsync();
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            _dataProvider.SetTokenResponse(response);

            return RedirectToAction(nameof(List), "Home");
        }

        public async Task<IActionResult> List()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_dataProvider.GoogleTokenResponse?.AccessToken}");

            var responseMessage = await httpClient.GetAsync(_dataProvider.DriveFileUrl);
            var stringContent = await responseMessage.Content.ReadAsStringAsync();
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DriveFileResponse>(stringContent);

            return View(data);
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
