using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace OrderbookAPI.Services
{
    public class OrderbookService
    {
        private ClientWebSocket _webSocket;

        public OrderbookService()
        {
            _webSocket = new ClientWebSocket();
            Task.Run(() => ConnectToValr());
        }

        public async Task ConnectToValr()
        {
            try
            {
                Uri valrWebSocketUri = new Uri("wss://api.valr.com/ws/trade");

                // Connect to the WebSocket
                await _webSocket.ConnectAsync(valrWebSocketUri, CancellationToken.None);

                // Subscribe to the orderbook updates//TODO
                var subscribeMessage = new
                {
                    type = "SUBSCRIBE",
                    channels = new[] { "FULL_ORDERBOOK_UPDATE" }
                };

                string subscribeJson = JsonSerializer.Serialize(subscribeMessage);
                var subscribeBytes = Encoding.UTF8.GetBytes(subscribeJson);

                await _webSocket.SendAsync(new ArraySegment<byte>(subscribeBytes), WebSocketMessageType.Text, true, CancellationToken.None);

                // Continuously listen to messages
                await ReceiveMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket connection failed: {ex.Message}");
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessOrderbookUpdate(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the client", CancellationToken.None);
                }
            }
        }

        private void ProcessOrderbookUpdate(string message)
        {
            // Parse the incoming WebSocket message and update the in-memory orderbook
            Console.WriteLine($"Received orderbook update: {message}");
            // Got from template, need to add in memory logic here
        }

        public decimal GetZARPriceForUSDT(decimal usdtQuantity)
        {
            // Implement the logic to calculate ZAR price based on the in-memory orderbook.
            // This is just a placeholder for demonstration.
            return usdtQuantity * 15;  // Example conversion rate (replace with real logic)
            // Not sure but I think all I need is price from message above and will use that to convert.
        }
    }
}
