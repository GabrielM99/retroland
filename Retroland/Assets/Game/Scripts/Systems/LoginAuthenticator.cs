using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Game
{
    public class LoginAuthenticator : NetworkAuthenticator
    {
        public struct LoginRequestMessage : NetworkMessage
        {
            public string username;
            public string password;
        }

        public struct LoginResponseMessage : NetworkMessage
        {
            public bool result;
        }

        public string username { get; set; }
        public string password { get; set; }
        public Action<LoginResponseMessage> onResponse { get; set; }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<LoginRequestMessage>(OnAuthRequestMessage, false);
        }

        public override void OnStopServer()
        {
            NetworkServer.UnregisterHandler<LoginRequestMessage>();
        }

        public override void OnServerAuthenticate(NetworkConnection conn)
        {

        }

        public async void OnAuthRequestMessage(NetworkConnection conn, LoginRequestMessage msg)
        {
            LoginResponseMessage response = new LoginResponseMessage();
            LoginData loginData = await Database.Login(msg.username, msg.password);

            bool result = loginData.result;
            response.result = result;

            if (result)
            {
                Network.RegisterUserConnection(conn, loginData.userID);
                ServerAccept(conn);
            }
            else
            {
                conn.isAuthenticated = false;
                StartCoroutine(DelayedDisconnect(conn, 1));
            }

            conn.Send(response);
        }

        IEnumerator DelayedDisconnect(NetworkConnection conn, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            ServerReject(conn);
        }

        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<LoginResponseMessage>((Action<LoginResponseMessage>)OnAuthResponseMessage, false);
        }

        public override void OnStopClient()
        {
            NetworkClient.UnregisterHandler<LoginResponseMessage>();
        }

        public override void OnClientAuthenticate(NetworkConnection conn)
        {
            LoginRequestMessage request = new LoginRequestMessage
            {
                username = username,
                password = password
            };

            conn.Send(request);
        }

        public void OnAuthResponseMessage(LoginResponseMessage msg)
        {
            if (msg.result)
            {
                ClientAccept(NetworkClient.connection);
            }
            else
            {
                ClientReject(NetworkClient.connection);
            }

            onResponse?.Invoke(msg);
            onResponse = null;
            username = null;
            password = null;
        }

        public void OnAuthResponseMessage(NetworkConnection conn, LoginResponseMessage msg)
        {
            OnAuthResponseMessage(msg);
        }
    }
}