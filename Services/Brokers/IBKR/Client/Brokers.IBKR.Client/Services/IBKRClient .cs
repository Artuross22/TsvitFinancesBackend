using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Models;
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

    public async Task<IBKRResponse<List<PortfolioPosition>>> GetPortfolioAsync(string accountId)
    {
        try
        {
            _logger.LogInformation($"Retrieving portfolio for account: {accountId}");
            var response = await _httpClient.GetAsync($"/v1/api/portfolio/{accountId}/positions/0");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var positions = JsonSerializer.Deserialize<List<PortfolioPosition>>(content, _jsonOptions);
                return new IBKRResponse<List<PortfolioPosition>> { Success = true, Data = positions };
            }
            return new IBKRResponse<List<PortfolioPosition>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving portfolio");
            return new IBKRResponse<List<PortfolioPosition>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<AccountSummary>>> GetAccountSummaryAsync(string accountId)
    {
        try
        {
            _logger.LogInformation($"Retrieving account balance: {accountId}");
            var response = await _httpClient.GetAsync($"/v1/api/portfolio/{accountId}/summary");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var summary = JsonSerializer.Deserialize<List<AccountSummary>>(content, _jsonOptions);
                return new IBKRResponse<List<AccountSummary>> { Success = true, Data = summary };
            }
            return new IBKRResponse<List<AccountSummary>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving account balance");
            return new IBKRResponse<List<AccountSummary>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<ContractInfo>>> SearchContractAsync(string symbol)
    {
        try
        {
            _logger.LogInformation($"Searching for contract: {symbol}");
            var response = await _httpClient.GetAsync($"/v1/api/iserver/secdef/search?symbol={symbol}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var contracts = JsonSerializer.Deserialize<List<ContractInfo>>(content, _jsonOptions);
                return new IBKRResponse<List<ContractInfo>> { Success = true, Data = contracts };
            }
            return new IBKRResponse<List<ContractInfo>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching for contract");
            return new IBKRResponse<List<ContractInfo>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<MarketData>>> GetMarketDataAsync(string conIds, string fields = "31,84,86")
    {
        try
        {
            _logger.LogInformation($"Retrieving market data for: {conIds}");
            var response = await _httpClient.GetAsync($"/v1/api/iserver/marketdata/snapshot?conids={conIds}&fields={fields}");
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

    public async Task<IBKRResponse<List<OrderResponse>>> PlaceLimitOrderAsync(string accountId, OrderRequest orderRequest)
    {
        try
        {
            _logger.LogInformation($"Placing limit order: {orderRequest.Side} {orderRequest.Quantity} at price {orderRequest.Price}");

            var orderData = new
            {
                orders = new[] { orderRequest }
            };

            var json = JsonSerializer.Serialize(orderData, _jsonOptions);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/v1/api/iserver/account/{accountId}/orders", stringContent);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orderResponse = JsonSerializer.Deserialize<List<OrderResponse>>(content, _jsonOptions);
                return new IBKRResponse<List<OrderResponse>> { Success = true, Data = orderResponse };
            }
            return new IBKRResponse<List<OrderResponse>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while placing limit order");
            return new IBKRResponse<List<OrderResponse>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<List<OrderResponse>>> PlaceMarketOrderAsync(string accountId, int conId, string side, int quantity)
    {
        var orderRequest = new OrderRequest
        {
            ConId = conId,
            OrderType = "MKT",
            Side = side,
            Quantity = quantity,
            Tif = "DAY"
        };

        return await PlaceLimitOrderAsync(accountId, orderRequest);
    }

    public async Task<IBKRResponse<object>> GetLiveOrdersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving live orders");
            var response = await _httpClient.GetAsync("/v1/api/iserver/account/orders");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orders = JsonSerializer.Deserialize<object>(content, _jsonOptions);
                return new IBKRResponse<object> { Success = true, Data = orders };
            }
            return new IBKRResponse<object> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving live orders");
            return new IBKRResponse<object> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<object>> CancelOrderAsync(string accountId, string orderId)
    {
        try
        {
            _logger.LogInformation($"Cancelling order: {orderId}");
            var response = await _httpClient.DeleteAsync($"/v1/api/iserver/account/{accountId}/order/{orderId}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<object>(content, _jsonOptions);
                return new IBKRResponse<object> { Success = true, Data = result };
            }
            return new IBKRResponse<object> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while cancelling order");
            return new IBKRResponse<object> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<object>> GetTradesAsync(string accountId, int days = 7)
    {
        try
        {
            _logger.LogInformation($"Retrieving trade history for {days} days");
            var response = await _httpClient.GetAsync($"/v1/api/iserver/account/trades?accountId={accountId}&days={days}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var trades = JsonSerializer.Deserialize<object>(content, _jsonOptions);
                return new IBKRResponse<object> { Success = true, Data = trades };
            }
            return new IBKRResponse<object> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving trade history");
            return new IBKRResponse<object> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<object>> ConfirmOrderAsync(string replyId, bool confirmed = true)
    {
        try
        {
            _logger.LogInformation($"Confirming order: {replyId}");

            var confirmData = new { confirmed = confirmed };
            var json = JsonSerializer.Serialize(confirmData, _jsonOptions);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/v1/api/iserver/reply/{replyId}", stringContent);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<object>(content, _jsonOptions);
                return new IBKRResponse<object> { Success = true, Data = result };
            }
            return new IBKRResponse<object> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while confirming order");
            return new IBKRResponse<object> { Success = false, Error = ex.Message };
        }
    }
}