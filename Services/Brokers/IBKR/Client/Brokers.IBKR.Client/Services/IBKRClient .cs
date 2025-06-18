using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Modelsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Brokers.IBKR.Client.Services;

public class IBKRClient
{
    private readonly HttpClient _httpClient;
    private readonly IBKROptions _options;
    private readonly ILogger<IBKRClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public IBKRClient(HttpClient httpClient, IOptions<IBKROptions> options, ILogger<IBKRClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromMinutes(_options.TimeoutMinutes);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);
    }

    public async Task<IBKRResponse<AuthStatus>> GetAuthStatusAsync()
    {
        try
        {
            _logger.LogInformation("Checking authorization status");

            var response = await _httpClient.PostAsync("/v1/api/iserver/auth/status", null);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authStatus = JsonSerializer.Deserialize<AuthStatus>(content, _jsonOptions);
                return new IBKRResponse<AuthStatus> { Success = true, Data = authStatus };
            }

            return new IBKRResponse<AuthStatus> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while checking authorization status");
            return new IBKRResponse<AuthStatus> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<Account>>> GetAccountsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving accounts");

            var response = await _httpClient.GetAsync("/portfolio/accounts");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var accounts = JsonSerializer.Deserialize<List<Account>>(content, _jsonOptions);
                return new IBKRResponse<List<Account>> { Success = true, Data = accounts };
            }

            return new IBKRResponse<List<Account>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving accounts");
            return new IBKRResponse<List<Account>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<Position>>> GetPositionsAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Retrieving positions for account {AccountId}", accountId);

            var response = await _httpClient.GetAsync($"/portfolio/{accountId}/positions/0");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var positions = JsonSerializer.Deserialize<List<Position>>(content, _jsonOptions);
                return new IBKRResponse<List<Position>> { Success = true, Data = positions };
            }

            return new IBKRResponse<List<Position>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving positions");
            return new IBKRResponse<List<Position>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<MarketData>>> GetMarketDataAsync(string conids)
    {
        try
        {
            _logger.LogInformation("Retrieving market data for {Conids}", conids);

            var response = await _httpClient.GetAsync($"/iserver/marketdata/snapshot?conids={conids}&fields=31,84,85,86");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var marketData = JsonSerializer.Deserialize<List<MarketData>>(content, _jsonOptions);
                return new IBKRResponse<List<MarketData>> { Success = true, Data = marketData };
            }

            return new IBKRResponse<List<MarketData>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving market data");
            return new IBKRResponse<List<MarketData>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<Contract>>> SearchContractsAsync(string symbol)
    {
        try
        {
            _logger.LogInformation("Searching for contracts for symbol {Symbol}", symbol);

            var response = await _httpClient.GetAsync($"/iserver/secdef/search?symbol={symbol}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var contracts = JsonSerializer.Deserialize<List<Contract>>(content, _jsonOptions);
                return new IBKRResponse<List<Contract>> { Success = true, Data = contracts };
            }

            return new IBKRResponse<List<Contract>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching for contracts");
            return new IBKRResponse<List<Contract>> { Success = false, Error = ex.Message };
        }
    }
}
