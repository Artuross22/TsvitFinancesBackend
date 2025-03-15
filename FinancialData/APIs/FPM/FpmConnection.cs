using FinancialData.APIs.FPM.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FinancialData.APIs.FPM;

public class FpmConnection
{
    protected readonly ApiSettings _apiSettings;

    public FpmConnection(IOptions<ApiSettings> apiSettingsOptions)
    {
        _apiSettings = apiSettingsOptions.Value;
    }

    private async Task<string> _connection(string url)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return null!;
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<CryptoData>> GetCrypto(string symbol)
    {
        string url = $"{_apiSettings.FPM.BaseUrl}quote/{symbol}?apikey={_apiSettings.FPM.Key}";

        var result = await _connection(url);

        if (result != null)
        {
            var dataList = JsonSerializer.Deserialize<List<CryptoData>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dataList ?? new List<CryptoData>();
        }

        return null!;
    }

    public async Task<List<ShareData>> GetShare(string symbol)
    {
        string url = $"{_apiSettings.FPM.BaseUrl}ratios/{symbol}?apikey={_apiSettings.FPM.Key}";

        var result = await _connection(url);

        if (result != null)
        {
            var dataList = JsonSerializer.Deserialize<List<ShareData>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dataList ?? new List<ShareData>();
        }

        return null!;
    }
}