using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ME.ECS.Network;
using Newtonsoft.Json;
using Project.Network;
using UnityEngine;

namespace Project.Modules
{
    using TState = ProjectState;

    /// <summary>
    /// We need to implement our own NetworkModule class without any logic just to catch your State type into ECS.Network
    /// You can use some overrides to setup history config for your project
    /// </summary>
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class NetworkModule : ME.ECS.Network.NetworkModule<TState>
    {
        private CustomWebSocketTransport transport;

        protected override int GetRPCOrder()
        {
            // Order all RPC packages by world id
            return this.world.id;
        }

        protected override ME.ECS.Network.NetworkType GetNetworkType()
        {
            // Initialize network with RunLocal and SendToNet
            return ME.ECS.Network.NetworkType.SendToNet | ME.ECS.Network.NetworkType.RunLocal;
        }

        protected override async void OnInitialize()
        {
            transport = new CustomWebSocketTransport("wss://dev.match.qubixinfinity.io/snake");
            var instance = (ME.ECS.Network.INetworkModuleBase)this;
            instance.SetTransporter(transport);
            // instance.SetSerializer(new CustomSerializer()); // ISerializer

            await transport.Connect();
            transport.SendStartGame();
        }
    }

    public class CustomWebSocketTransport : ITransporter
    {
        private readonly ClientWebSocket webSocket;
        private readonly Uri serverUri;
        private readonly CancellationTokenSource cancellationTokenSource;

        private int eventsSentCount;
        private int eventsBytesSentCount;
        private int eventsReceivedCount;
        private int eventsBytesReceivedCount;

        public CustomWebSocketTransport(string serverUrl)
        {
            serverUri = new Uri(serverUrl);
            webSocket = new ClientWebSocket();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Connect()
        {
            if (webSocket.State is WebSocketState.Connecting or WebSocketState.Open)
            {
                Debug.LogWarning("WebSocket is already connecting or open.");
                return;
            }

            try
            {
                await webSocket.ConnectAsync(serverUri, cancellationTokenSource.Token);
                Debug.Log("Connected");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to connect to WebSocket server: {e.Message}");
            }
        }

        public async Task Disconnect()
        {
            if (webSocket.State is WebSocketState.Closed or WebSocketState.CloseReceived or WebSocketState.CloseSent)
            {
                Debug.LogWarning("WebSocket is already closed or closing.");
                return;
            }

            try
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client.",
                    cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to close WebSocket connection: {e.Message}");
            }
            finally
            {
                webSocket.Dispose();
                cancellationTokenSource.Cancel();
            }
        }

        public bool IsConnected()
        {
            return webSocket.State == WebSocketState.Open;
        }

        public async void SendStartGame()
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(new StartGameMessage());
                var buffer = Encoding.UTF8.GetBytes(jsonData);
                Debug.Log("Отправка StartGame");
                Send(buffer);

                Debug.Log("Отправлено, ожидаем ответ");
                
                // Получение ответа от сервера
                var receiveBuffer = new byte[1024];
                var result =
                    await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationTokenSource.Token);
                var response = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                Console.WriteLine("Получен ответ от сервера: " + response);
            }
            catch (Exception e)
            {
                Debug.LogError("Catched " + e.Message);
            }
        }

        public async void Send(byte[] bytes)
        {
            if (!IsConnected())
            {
                Debug.LogWarning("WebSocket is not open. Cannot send data.");
                return;
            }

            try
            {
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true,
                    cancellationTokenSource.Token);
                eventsSentCount++;
                eventsBytesSentCount += bytes.Length;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send data via WebSocket: {e.Message}");
            }
        }

        public async void SendSystem(byte[] bytes)
        {
            if (!IsConnected())
            {
                Debug.LogWarning("WebSocket is not open. Cannot send system data.");
                return;
            }

            try
            {
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true,
                    cancellationTokenSource.Token);
                eventsSentCount++;
                eventsBytesSentCount += bytes.Length;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send system data via WebSocket: {e.Message}");
            }
        }

        public byte[] Receive()
        {
            if (!IsConnected())
            {
                Debug.LogWarning("WebSocket is not open. Cannot receive data.");
                return null;
            }

            var buffer = new byte[1024];
            var receivedData = new List<byte>();

            try
            {
                WebSocketReceiveResult result;
                do
                {
                    result = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token)
                        .GetAwaiter().GetResult();
                    receivedData.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));
                    eventsReceivedCount++;
                    eventsBytesReceivedCount += result.Count;
                } while (!result.EndOfMessage);

                return receivedData.ToArray();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to receive data via WebSocket: {e.Message}");
                return null;
            }
        }

        public int GetEventsSentCount()
        {
            return eventsSentCount;
        }

        public int GetEventsBytesSentCount()
        {
            return eventsBytesSentCount;
        }

        public int GetEventsReceivedCount()
        {
            return eventsReceivedCount;
        }

        public int GetEventsBytesReceivedCount()
        {
            return eventsBytesReceivedCount;
        }
    }
}