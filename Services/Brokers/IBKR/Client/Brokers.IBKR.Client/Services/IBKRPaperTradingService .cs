using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace Brokers.IBKR.Client.Services;

public class IBKRPaperTradingService
{
    private readonly IBKROptions _options;
    private readonly ILogger<IBKRPaperTradingService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public IBKRPaperTradingService(HttpClient httpClient, IBKROptions options, ILogger<IBKRPaperTradingService> logger)
    {
        _options = options;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromMinutes(_options.TimeoutMinutes);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);
    }

    public async Task<IBKRResponse<List<PaperAccount>>> GetPaperAccountsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving Paper Trading accounts");
            var response = await _httpClient.GetAsync("/v1/api/iserver/accounts");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var accounts = JsonSerializer.Deserialize<AccountsResponse>(content, _jsonOptions);
                var paperAccounts = accounts?.Accounts?.Where(a => a.AccountId.StartsWith("DU")).ToList() ?? new List<PaperAccount>();
                return new IBKRResponse<List<PaperAccount>> { Success = true, Data = paperAccounts };
            }
            return new IBKRResponse<List<PaperAccount>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving Paper Trading accounts");
            return new IBKRResponse<List<PaperAccount>> { Success = false, Error = ex.Message };
        }
    }

    public bool IsPaperAccount(string accountId)
    {
        return accountId.StartsWith("DU");
    }

}