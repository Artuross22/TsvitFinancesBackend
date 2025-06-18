using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Modelsl;

namespace Brokers.IBKR.Client.Interfaces;

//public interface IIBKRClient
//{
//    Task<IBKRResponse<AuthStatus>> GetAuthStatusAsync();
//    Task<IBKRResponse<InitializeSessionResponse>> InitializeSessionAsync();
//    Task<IBKRResponse<List<Account>>> GetAccountsAsync();
//    Task<IBKRResponse<List<Position>>> GetPositionsAsync(string accountId);
//    Task<IBKRResponse<List<MarketData>>> GetMarketDataAsync(string conids);
//    Task<IBKRResponse<List<Contract>>> SearchContractsAsync(string symbol);
//    Task<IBKRResponse<PlaceOrderResponse>> PlaceOrderAsync(string accountId, OrderRequest order);
//    Task<IBKRResponse<CancelOrderResponse>> CancelOrderAsync(string accountId, string orderId); 
//    Task<IBKRResponse<List<Order>>> GetOrdersAsync(string accountId);
//}