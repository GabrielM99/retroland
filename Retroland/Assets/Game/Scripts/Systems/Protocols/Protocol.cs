using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public abstract class Protocol<TRequest, TResponse> where TRequest : struct, NetworkMessage where TResponse : struct, NetworkMessage
    {
        private Action<TResponse> onResponse { get; set; }

        public Protocol()
        {
            Network.OnClientStartHandler += () => NetworkClient.RegisterHandler<TResponse>(OnResponseMessage);
            Network.OnServerStartHandler += () => NetworkServer.RegisterHandler<TRequest>(OnRequestMessage);
        }

        public abstract Task<TResponse> Validate(NetworkConnection connection, TRequest request);

        public void Execute(TRequest request, Action<TResponse> response = null)
        {
            onResponse -= response;
            onResponse += response;
            NetworkClient.Send(request);
        }

        private async void OnRequestMessage(NetworkConnection connection, TRequest request)
        {
            TResponse response = await Validate(connection, request);
            connection.Send(response);
        }

        private void OnResponseMessage(TResponse response)
        {
            onResponse?.Invoke(response);
            onResponse = null;
        }
    }
}
