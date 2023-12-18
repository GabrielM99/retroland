using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Game
{
    public static class Database
    {
        private struct HashSalt
        {
            public string hash;
            public string salt;

            public HashSalt(string hash, string salt)
            {
                this.hash = hash;
                this.salt = salt;
            }
        }

        private struct Query
        {
            public string text;
            public object[] parameters;

            public Query(string text, params object[] parameters)
            {
                this.text = text;
                this.parameters = parameters;
            }
        }

        private const int HashSize = 256;
        private const int HashIterations = 10000;
        private const int SaltSize = 64;

        private static MySqlConnection connection { get; set; }

        public static void Connect(string server, string database, string user, string password, bool pooling = true)
        {
            if (connection == null)
            {
                connection = new MySqlConnection($"server={server};database={database};user={user};password={password}");
                connection.Open();
                Debug.Log("Connected to database!");
            }
            else
            {
                Debug.LogError("A connection to the database is already established. Use Disconnect() first if it was intentional");
            }
        }

        public static void Disconnect()
        {
            connection.Close();
            connection = null;
            Debug.Log("Disconnected from database!");
        }

        public static async Task<bool> Register(string username, string password)
        {
            if (await Read(new Query("SELECT username FROM users WHERE username = ?", username)))
            {
                return false;
            }

            HashSalt hashSalt = HashPassword(password);

            await Execute(new Query($"INSERT INTO users (username, hash, salt) VALUES (?, ?, ?)", username, hashSalt.hash, hashSalt.salt));

            return true;
        }

        public static async Task<LoginData> Login(string username, string password)
        {
            LoginData data = new LoginData();

            await Read(new Query("SELECT id, hash, salt FROM users WHERE username = ?", username), (reader) =>
            {
                while (reader.Read())
                {
                    HashSalt hashSalt = new HashSalt((string)reader["hash"], (string)reader["salt"]);

                    if (VerifyPassword(password, hashSalt))
                    {
                        data.userID = (int)reader["id"];
                        data.result = true;
                        break;
                    }
                }
            });

            return data;
        }

        public static async Task<CreateCharacterData> CreateCharacter(int userID, string name)
        {
            CreateCharacterData data = new CreateCharacterData();

            if (!await Read(new Query("SELECT name FROM characters WHERE name = ?", name)))
            {
                await Execute(new Query("INSERT INTO characters (user_id, name) VALUES (?, ?)", userID, name));
                data.result = true;
            }

            return data;
        }

        public static async Task<CharacterData[]> GetCharacters(int userID)
        {
            List<CharacterData> data = new List<CharacterData>();

            await Read(new Query("SELECT id, name FROM characters WHERE user_id = ?", userID), (reader) =>
            {
                while (reader.Read())
                {
                    data.Add(new CharacterData()
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    });
                }
            });

            return data.ToArray();
        }

        private static async Task<int> Execute(Query query)
        {
            using (MySqlCommand command = CreateCommand(query))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }

        private static async Task<bool> Read(Query query, Action<DbDataReader> onRead = null)
        {
            using (MySqlCommand command = CreateCommand(query))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    onRead?.Invoke(reader);
                    return reader.HasRows;
                }
            }
        }

        private static MySqlCommand CreateCommand(Query query)
        {
            MySqlCommand command = new MySqlCommand(query.text, connection);
            command.Prepare(query.parameters);
            return command;
        }

        private static HashSalt HashPassword(string password)
        {
            byte[] saltBytes = new byte[SaltSize];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            Rfc2898DeriveBytes hashBytes = new Rfc2898DeriveBytes(password, saltBytes, HashIterations);
            string hash = Convert.ToBase64String(hashBytes.GetBytes(HashSize));

            return new HashSalt(hash, salt);
        }

        private static bool VerifyPassword(string password, HashSalt hashSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(hashSalt.salt);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, HashIterations);
            string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(HashSize));
            return hash == hashSalt.hash;
        }
    }
}
