using System.Web;

namespace GoogleDriveFileListing.Services;

public class EncodedUrlBuilder(string baseUrl)
{

    private Dictionary<string, string> Params = [];

    public EncodedUrlBuilder AddParam(string key, string value)
    {
        Params.Add(key, value);
        return this;
    }

    public string Build()
    {
        var encodedKeyValuePairs = new List<string>();
        foreach (var kvp in Params)
        {
            encodedKeyValuePairs.Add($"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}");
        }

        var queryString = string.Join("&", encodedKeyValuePairs);
        return baseUrl + "?" + queryString;
    }
}
