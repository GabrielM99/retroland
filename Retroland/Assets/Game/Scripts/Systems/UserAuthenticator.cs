using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Game
{
    public class UserAuthenticator : NetworkAuthenticator
    {
        public enum UserRequestType
        {
            Register,
            Login
        }

        public struct UserRequestMessage : NetworkMessage
        {
            public string username;
            public string password;
            public UserRequestType requestType;
        }

        public struct UserResponseMessage : NetworkMessage
        {
            public bool result;
            public UserRequestType requestType;
        }

        public string username { get; set; }
        public string password { get; set; }
        public UserRequestType requestType { get; set; }
        public Action<UserResponseMessage> onResponse { get; set; }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<UserRequestMessage>(OnAuthRequestMessage, false);
        }

        public override void OnStopServer()
        {
            NetworkServer.UnregisterHandler<UserRequestMessage>();
        }

        public override void OnServerAuthenticate(NetworkConnection conn)
        {

        }

        public void OnAuthRequestMessage(NetworkConnection conn, UserRequestMessage msg)
        {
            switch (msg.requestType)
            {
                case UserRequestType.Register:
                    OnRegisterRequestMessage(conn, msg);
                    break;
                case UserRequestType.Login:
                    OnLoginRequestMessage(conn, msg);
                    break;
            }
        }

        private async void OnRegisterRequestMessage(NetworkConnection conn, UserRequestMessage msg)
        {
            UserResponseMessage response = new UserResponseMessage();
            response.requestType = msg.requestType;

            if (await Database.Register(msg.username, msg.password))
            {
                response.result = true;
            }
            else
            {
                conn.isAuthenticated = false;
                StartCoroutine(DelayedDisconnect(conn, 1));
            }

            conn.Send(response);
        }

        private async void OnLoginRequestMessage(NetworkConnection conn, UserRequestMessage msg)
        {
            UserResponseMessage response = new UserResponseMessage();
            LoginData loginData = await Database.Login(msg.username, msg.password);

            bool result = loginData.result;
            
            response.result = result;
            response.requestType = msg.requestType;

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
            NetworkClient.RegisterHandler<UserResponseMessage>((Action<UserResponseMessage>)OnAuthResponseMessage, false);
        }

        public override void OnStopClient()
        {
            NetworkClient.UnregisterHandler<UserResponseMessage>();
        }

        public override void OnClientAuthenticate(NetworkConnection conn)
        {
            UserRequestMessage request = new UserRequestMessage
            {
                username = username,
                password = password,
                requestType = requestType
            };

            conn.Send(request);
        }

        public void OnAuthResponseMessage(UserResponseMessage msg)
        {
            switch (msg.requestType)
            {
                case UserRequestType.Register:
                    OnRegisterResponseMessage(msg);
                    break;
                case UserRequestType.Login:
                    OnLoginResponseMessage(msg);
                    break;
            }
        }

        private void OnRegisterResponseMessage(UserResponseMessage msg)
        {
            ClientReject(NetworkClient.connection);
            onResponse?.Invoke(msg);
            onResponse = null;
            username = null;
            password = null;
        }

        private void OnLoginResponseMessage(UserResponseMessage msg)
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

        public void OnAuthResponseMessage(NetworkConnection conn, UserResponseMessage msg)
        {
            OnAuthResponseMessage(msg);
        }
    }
}