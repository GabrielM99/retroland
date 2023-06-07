using System;
using System.Collections.Generic;
using System.IO;
using Game.Entities;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Game.LoginAuthenticator;

namespace Game
{
    public class Network : NetworkManager
    {
        [Header("Game")]
        [Scene][SerializeField] private string _playScene;

        public static Network Instance { get; private set; }
        public static RegisterProtocol RegisterProtocol { get; private set; }
        public static CreateCharacterProtocol CreateCharacterProtocol { get; private set; }
        public static GetCharactersProtocol GetCharactersProtocol { get; private set; }
        public static PlayProtocol PlayProtocol { get; private set; }
        public static Action OnServerStartHandler { get; set; }
        public static Action OnClientStartHandler { get; set; }

        private static LoginAuthenticator Authenticator { get; set; }
        private static Dictionary<NetworkConnection, UserData> UserDataByConnection { get; set; }
        private static Dictionary<int, NetworkConnection> ConnectionsByUserID { get; set; }

        private static string PlayScene { get => Instance._playScene; }

        public override void Awake()
        {
            base.Awake();
            Instance = singleton as Network;
            Authenticator = authenticator as LoginAuthenticator;
            RegisterProtocol = new RegisterProtocol();
            CreateCharacterProtocol = new CreateCharacterProtocol();
            GetCharactersProtocol = new GetCharactersProtocol();
            PlayProtocol = new PlayProtocol();
            UserDataByConnection = new Dictionary<NetworkConnection, UserData>();
            ConnectionsByUserID = new Dictionary<int, NetworkConnection>();
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        public override void Start()
        {
            base.Start();
        }

        public static void Login(string username, string password, Action<LoginResponseMessage> onResponse)
        {
            Authenticator.username = username;
            Authenticator.password = password;
            Authenticator.onResponse -= onResponse;
            Authenticator.onResponse += onResponse;

            if (Application.isEditor)
            {
                if (NetworkServer.active)
                {
                    Instance.StopHost();
                }

                Instance.StartHost();
                //Instance.StartClient();
            }
            else
            {
                Instance.StartClient();
            }
        }

        public static void Logout()
        {
            if (Application.isEditor)
            {
                Instance.StopHost();
            }
            else
            {
                Instance.StopClient();
            }

            SceneManager.LoadScene(Instance.offlineScene);
        }

        public static void LoadPlayScene()
        {
            SceneManager.LoadScene(PlayScene);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            OnClientStartHandler?.Invoke();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            Logout();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            OnServerStartHandler?.Invoke();

            if (!Application.isEditor)
            {
                LoadPlayScene();
            }
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Player player = Instantiate(playerPrefab).GetComponent<Player>();

            if (player == null)
            {
                Debug.LogError($"Player Prefab must be of type {typeof(Player)}.");
                return;
            }

            UserData userData = GetUserData(conn);

            if (userData == null)
            {
                Debug.LogError("Tried to spawn the player of a non-registered user (UserData is null).");
                return;
            }

            CharacterData characterData = userData.CharacterData;

            player.CharacterData = characterData;
            player.name = $"{characterData.name} [connId={conn.connectionId}]";

            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            UnregisterUserConnection(conn);
            base.OnServerDisconnect(conn);
        }

        public static void RegisterUserConnection(NetworkConnection connection, int userID)
        {
            UnregisterUserConnection(connection);

            if (ConnectionsByUserID.TryGetValue(userID, out NetworkConnection existingConnection))
            {
                UnregisterUserConnection(existingConnection);
            }

            UserData userData = new UserData(userID);

            ConnectionsByUserID.Add(userID, connection);
            UserDataByConnection.Add(connection, userData);
        }

        public static UserData GetUserData(NetworkConnection connection)
        {
            if (UserDataByConnection.TryGetValue(connection, out UserData userData))
            {
                return userData;
            }

            return null;
        }

        private static void UnregisterUserConnection(NetworkConnection connection)
        {
            if (UserDataByConnection.TryGetValue(connection, out UserData userData))
            {
                ConnectionsByUserID.Remove(userData.ID);
                UserDataByConnection.Remove(connection);
                connection.Disconnect();
            }
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (NetworkClient.active)
            {
                if (scene.path == PlayScene)
                {
                    NetworkClient.AddPlayer();
                }
            }
        }
    }
}
