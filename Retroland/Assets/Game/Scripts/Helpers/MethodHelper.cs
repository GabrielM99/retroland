using System.Collections;
using System.Collections.Generic;
using Mirror;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Game
{
    public static class MethodHelper
    {
        private const string MySqlParameterPlaceholder = "?";

        public static string ReplaceFirst(this string s, string oldValue, string newValue)
        {
            int startindex = s.IndexOf(oldValue);

            if (startindex == -1)
            {
                return s;
            }

            return s.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
        }

        public static void Prepare(this MySqlCommand command, params object[] parameters)
        {
            command.Prepare();

            for (int i = 0; i < parameters.Length; i++)
            {
                string key = MySqlParameterPlaceholder + i;
                object value = parameters[i];

                command.CommandText.ReplaceFirst(MySqlParameterPlaceholder, key);
                command.Parameters.AddWithValue(key, value);
            }
        }
    }
}
