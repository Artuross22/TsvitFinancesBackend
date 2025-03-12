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

    public async Task Get(string symbol)
    {
        string url = $"{_apiSettings.FPM.BaseUrl}{symbol}?apikey={_apiSettings.FPM.Key}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                //return JsonSerializer.Deserialize<List<Financial>>;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
}
