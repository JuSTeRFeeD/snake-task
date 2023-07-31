using System;
using System.Threading.Tasks;
using ME.ECS;
using NativeWebSocket;
using Newtonsoft.Json;
using Project.Components;
using Project.Markers;
using Project.Modules;
using UnityEngine;

namespace Project.Network
{
    public class WebSocketTransport : MonoBehaviour
    {
        private string baseUrl;

        private WebSocket webSocket;
        
        public int SentCount  { get; private set; }
        public int SentBytesCount  { get; private set; }
        public int ReceivedCount  { get; private set; }
        public int ReceivedBytesCount  { get; private set; }

        public void SetBaseUrl(string url)
        {
            baseUrl = url;
        }

        private async void Start()
        {
            webSocket = new WebSocket(baseUrl);
            webSocket.OnOpen += OnConnect;
            webSocket.OnError += (e) => Debug.LogError($"[WebSocket] OnError: {e}");
            webSocket.OnMessage += OnMessage;
            webSocket.OnClose += (e) => Debug.Log($"[WebSocket] On Close Code: {e}");

            await webSocket.Connect();
        }

        private async void OnApplicationQuit()
        {
            await SendEnd();
            await webSocket.Close();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocket.DispatchMessageQueue();
#endif
        }

        private void LateUpdate()
        {
            var world = Worlds.currentWorld;
            if (world.HasMarker<UpdateProgressMarker>())
            {
                SendUpdate(world.GetSharedData<GameInfo>());
                world.RemoveMarker<UpdateProgressMarker>();
            }
            if (world.HasMarker<GameEndMarker>())
            {
                SendEnd();
                world.RemoveMarker<GameEndMarker>();
            }
        }

        private async void OnConnect()
        {
            await webSocket.SendText(JsonConvert.SerializeObject(new StartGameMessage()));
        }

        private void OnMessage(byte[] bytes)
        {
            ReceivedBytesCount += bytes.Length;
            ReceivedCount++;

            var world = Worlds.currentWorld;
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            var messageType = JsonConvert.DeserializeObject<Message>(message)?.type;
            
            Debug.Log("received message: " + message); 

            switch (messageType)
            {
                case "game-created":
                    var gameCreated = JsonConvert.DeserializeObject<GameCreatedMessage>(message);
                    world.AddMarker(new GameCreatedMarker() { gameId = gameCreated.payload.id });
                    break;
                case "game-ended":
                    var gameEnded = JsonConvert.DeserializeObject<GameEndedMessage>(message);
                    
                    var startTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(gameEnded.payload.startAt));
                    var endTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(gameEnded.payload.finishAt));
                    var diff = endTime - startTime;
                    
                    world.AddMarker(new GameEndedMarker()
                    {
                        snakeLength = gameEnded.payload.snakeLength,
                        collectedApples = gameEnded.payload.collectedApples,
                        playedTime = diff.Seconds
                    });
                    break;
                default:
                    Debug.LogError($"[WebSocketTransporter] Type: {messageType}. Message: {message}");
                    break;
            }
        }

        public bool IsConnected() => webSocket.State == WebSocketState.Open;

        private void SendUpdate(GameInfo gameInfo)
        {
            if (!IsConnected()) return;
            
            var data = new CollectAppleMessage()
            {
                payload = new CollectAppleMessage.Payload()
                {
                    game_id = gameInfo.gameId,
                    appleCount = gameInfo.appleCount,
                    snakeLength = gameInfo.snakeLength,
                }
            };
            
            webSocket.SendText(JsonConvert.SerializeObject(data));
        }
        
        private async Task SendEnd()
        {
            if (!IsConnected()) return;

            var world = Worlds.currentWorld;
            var data = new GameEndMessage()
            {
                payload = new GameEndMessage.Payload()
                {
                    game_id = world.GetSharedData<GameInfo>().gameId
                }
            };
            
            await webSocket.SendText(JsonConvert.SerializeObject(data));
        }
    }
}