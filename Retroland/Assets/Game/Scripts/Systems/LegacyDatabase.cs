using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class LegacyDatabase : Singleton<LegacyDatabase>
    {
        [SerializeField] private string _name;
        [SerializeField] private string _hostname;

        public static string SessionToken { get; private set; }

        private static string Name { get => Instance._name; }
        private static string Hostname { get => Instance._hostname; }

        public static void Post(string uri, WWWForm form, Action<UnityWebRequest> onResult = null)
        {
            Request(UnityWebRequest.Post(GetFullURI(uri), form), onResult);
        }

        public static void Login(string username, string password, Action<UnityWebRequest> onResult = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("username", username);
            form.AddField("password", password);

            Post("login.php", form, (request) =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    SessionToken = request.downloadHandler.text;
                }

                onResult?.Invoke(request);
            });
        }

        public static void Register(string username, string password, Action<UnityWebRequest> onResult = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("username", username);
            form.AddField("password", password);

            Post("register.php", form, onResult);
        }

        public static void CreateCharacter(string name, Action<UnityWebRequest> onResult = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("token", SessionToken);
            form.AddField("name", name);

            Post("create_character.php", form, onResult);
        }

        public static void GetCharacterData(string token, Action<UnityWebRequest> onResult = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("token", token);

            Post("get_characters.php", form, onResult);
        }

        public static void GetCharacterData(Action<UnityWebRequest> onResult = null)
        {
            GetCharacterData(SessionToken, onResult);
        }

        public static void SaveCharacter(string token, CharacterData data, Action<UnityWebRequest> onResult = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("token", token);
            form.AddField("data", JsonUtility.ToJson(data));

            Post("save_character.php", form, onResult);
        }

        public static void Logout()
        {
            SessionToken = null;
        }

        private static string GetFullURI(string uri)
        {
            return $"http://{Hostname}/{Name}/{uri}";
        }

        private static void Request(UnityWebRequest request, Action<UnityWebRequest> onResult = null)
        {
            Instance.StartCoroutine(RequestRoutine(request, onResult));
        }

        private static IEnumerator RequestRoutine(UnityWebRequest request, Action<UnityWebRequest> onResult = null)
        {
            using (request)
            {
                yield return request.SendWebRequest();
                onResult?.Invoke(request);
            }
        }
    }
}
