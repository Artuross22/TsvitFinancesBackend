using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Services;

public class IBKRClient
{
    protected readonly HttpClient _httpClient;
    protected readonly IBKROptions _options;
    protected readonly ILogger<IBKRClient> _logger;
    protected readonly JsonSerializerSettings _jsonSettings;

    public IBKRClient(HttpClient httpClient, IOptions<IBKROptions> options, ILogger<IBKRClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };
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
                var authStatus = JsonConvert.DeserializeObject<AuthStatus>(content, _jsonSettings);
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

    public async Task<IBKRResponse<AccountInfo>> GetCurrentAccountAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving Paper Trading accounts");
            var response = await _httpClient.GetAsync("/v1/api/iserver/accounts");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var accountsResponse = JsonConvert.DeserializeObject<AccountsResponse>(content, _jsonSettings);
                
                var account = accountsResponse?.Accounts?
                    .Select(accountId => new AccountInfo
                    {
                        AccountId = accountId,
                        Alias = accountsResponse.Aliases.GetValueOrDefault(accountId, accountId),
                        IsPaperAccount = accountId.StartsWith("DU")
                    })
                    .Single();

                return new IBKRResponse<AccountInfo> { Success = true, Data = account };
            }
            
            return new IBKRResponse<AccountInfo> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving Paper Trading accounts");
            return new IBKRResponse<AccountInfo> { Success = false, Error = ex.Message };
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
                var accounts = JsonConvert.DeserializeObject<List<Account>>(content, _jsonSettings);
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
                var positions = JsonConvert.DeserializeObject<List<PortfolioPosition>>(content, _jsonSettings);
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
                var summary = JsonConvert.DeserializeObject<List<AccountSummary>>(content, _jsonSettings);
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
                var contracts = JsonConvert.DeserializeObject<List<ContractInfo>>(content, _jsonSettings);
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
                var marketData = JsonConvert.DeserializeObject<List<MarketData>>(content, _jsonSettings);
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
            _logger.LogInformation($"Placing limit order: {orderRequest.Side} {orderRequest.Quantity} at price {orderRequest.Price}. Account: {accountId}");

            var payload = new
            {
                orders = new List<OrderRequest> { orderRequest }
            };
            
            var json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/v1/api/iserver/account/{accountId}/orders", stringContent);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var orderResponse = JsonConvert.DeserializeObject<List<OrderResponse>>(content, _jsonSettings);
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

    public async Task<IBKRResponse<List<OrderResponse>>> PlaceMarketOrderAsync(string accountId, string conId, string side, int quantity)
    {
        var orderRequest = new OrderRequest
        {
            Conid = int.Parse(conId),
            OrderType = "MKT",
            Side = side,
            Quantity = quantity,
            Tif = "DAY"
        };

        return await PlaceLimitOrderAsync(accountId, orderRequest);
    }

    public async Task<IBKRResponse<List<Order>>> GetLiveOrdersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving live orders");
            var response = await _httpClient.GetAsync("/v1/api/iserver/account/orders");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var orders = JsonConvert.DeserializeObject<List<Order>>(content, _jsonSettings);
                return new IBKRResponse<List<Order>> { Success = true, Data = orders };
            }
            return new IBKRResponse<List<Order>> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving live orders");
            return new IBKRResponse<List<Order>> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<OrderResponse>> CancelOrderAsync(string accountId, string orderId)
    {
        try
        {
            _logger.LogInformation($"Cancelling order: {orderId}");
            var response = await _httpClient.DeleteAsync($"/v1/api/iserver/account/{accountId}/order/{orderId}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderResponse>(content, _jsonSettings);
                return new IBKRResponse<OrderResponse> { Success = true, Data = result };
            }
            return new IBKRResponse<OrderResponse> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while cancelling order");
            return new IBKRResponse<OrderResponse> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<TradeHistoryResponse>> GetTradesAsync(string accountId, int days = 7)
    {
        try
        {
            _logger.LogInformation($"Retrieving trade history for {days} days");
            var response = await _httpClient.GetAsync($"/v1/api/iserver/account/trades?accountId={accountId}&days={days}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var trades = JsonConvert.DeserializeObject<TradeHistoryResponse>(content, _jsonSettings);
                return new IBKRResponse<TradeHistoryResponse> { Success = true, Data = trades }; 
            }
            return new IBKRResponse<TradeHistoryResponse> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving trade history");
            return new IBKRResponse<TradeHistoryResponse> { Success = false, Error = ex.Message };
        }
    }

    public async Task<IBKRResponse<OrderResponse>> ConfirmOrderAsync(string replyId, bool confirmed = true)
    {
        try
        {
            _logger.LogInformation($"Confirming order: {replyId}");
            var confirmData = new { confirmed = confirmed };
            var json = JsonConvert.SerializeObject(confirmData, _jsonSettings);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/v1/api/iserver/reply/{replyId}", stringContent);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<OrderResponse>(content, _jsonSettings);
                return new IBKRResponse<OrderResponse> { Success = true, Data = result };
            }
            return new IBKRResponse<OrderResponse> { Success = false, Error = content };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while confirming order");
            return new IBKRResponse<OrderResponse> { Success = false, Error = ex.Message };
        }
    }
}