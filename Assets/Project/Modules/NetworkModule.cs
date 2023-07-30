using ME.ECS.Network;
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
    public sealed class NetworkModule : NetworkModule<TState>
    {
        private CustomWebSocketTransport transport;
        private const string BaseUrl = "wss://dev.match.qubixinfinity.io/snake";

        protected override int GetRPCOrder()
        {
            // Order all RPC packages by world id
            return world.id;
        }

        protected override NetworkType GetNetworkType()
        {
            // Initialize network with RunLocal and SendToNet
            return NetworkType.SendToNet | NetworkType.RunLocal;
        }

        protected override void OnInitialize()
        {
            transport = new CustomWebSocketTransport(BaseUrl);
            var instance = (INetworkModuleBase)this;
            instance.SetTransporter(transport);
            // instance.SetSerializer(new CustomSerializer()); // ISerializer
        }
    }

    public class CustomWebSocketTransport : ITransporter
    {
        private readonly WebSocketTransport webSocketTransport;

        public CustomWebSocketTransport(string baseUrl)
        {
            var netObj = new GameObject("WebSocketTransport");
            webSocketTransport = netObj.AddComponent<WebSocketTransport>();
            webSocketTransport.SetBaseUrl(baseUrl);
        }

        public bool IsConnected()
        {
            return webSocketTransport.IsConnected();
        }

        public void Send(byte[] bytes)
        {
        }

        public void SendSystem(byte[] bytes)
        {
        }

        public byte[] Receive()
        {
            return null;
        }

        public int GetEventsSentCount()
        {
            return webSocketTransport.SentCount;
        }

        public int GetEventsBytesSentCount()
        {
            return webSocketTransport.SentBytesCount;
        }

        public int GetEventsReceivedCount()
        {
            return webSocketTransport.ReceivedCount;
        }

        public int GetEventsBytesReceivedCount()
        {
            return webSocketTransport.ReceivedBytesCount;
        }
    }
}