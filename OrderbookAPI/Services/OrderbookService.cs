using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OrderbookAPI.Models;
using OrderbookAPI.Services;

namespace OrderBookAPI.Services
{
    public class OrderbookService : IHostedService
    {
        private readonly InMemoryOrderBook _orderBook;
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();
        private readonly string _webSocketUrl;
        private readonly string _subscriptionEvent;

        /// <summary>
        /// OrderbookService
        /// </summary>
        /// <param name="orderBook">orderBook</param>
        /// <param name="configuration">configuration</param>
        public OrderbookService(InMemoryOrderBook orderBook, IConfiguration configuration)
        {
            _orderBook = orderBook;

            // Access WebSocketUrl and SubscriptionEvent from appsettings.json
            _webSocketUrl = configuration["OrderbookService:WebSocketUrl"];
            _subscriptionEvent = configuration["OrderbookService:SubscriptionEvent"];
        }

        /// <summary>
        /// StartAsync
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the WebSocket connection asynchronously
            _ = Task.Run(async () => await ConnectToWebSocket());
        }


        /// <summary>
        /// StopAsync
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Task</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Service stopping", CancellationToken.None);
        }

        /// <summary>
        /// ConnectToWebSocket
        /// </summary>
        private async Task ConnectToWebSocket()
        {
            try
            {
                // Connect to WebSocket using URL from appsettings.json
                await _webSocket.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);
                Console.WriteLine("Connected to WebSocket!");

                var subscribeMessage = new
                {
                    type = "SUBSCRIBE",
                    subscriptions = new[]
                    {
                        new
                        {
                            @event = _subscriptionEvent,
                            pairs = new[] { "USDTZAR" }
                        }
                    }
                };

                var jsonMessage = JsonSerializer.Serialize(subscribeMessage);
                var bytes = Encoding.UTF8.GetBytes(jsonMessage);
                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                await ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to WebSocket: {ex.Message}");
            }
        }

        /// <summary>
        /// ReceiveMessagesAsync
        /// </summary>
        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessOrderBookUpdate(message);
                }
            }
        }

        /// <summary>
        /// ProcessOrderBookUpdate
        /// </summary>
        /// <param name="message">message</param>
        private void ProcessOrderBookUpdate(string message)
        {
            try
            {
                // First, deserialize only the type of the message to check it
                var messageType = JsonSerializer.Deserialize<MessageType>(message);

                // Handle only "FULL_ORDERBOOK_UPDATE" messages
                if (messageType.type == "FULL_ORDERBOOK_UPDATE")
                {
                    // Deserialize the full message only if it's an update
                    var orderBookUpdate = JsonSerializer.Deserialize<OrderBookUpdate>(message);
                    _orderBook.UpdateOrderBook(orderBookUpdate);
                    Console.WriteLine("Orderbook updated.");
                }
                else
                {
                    Console.WriteLine($"Received message of type: {messageType.type}, ignoring.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing orderbook update: {ex.Message}");
            }
        }
    }
}
