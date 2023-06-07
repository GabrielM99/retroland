using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class SerializationHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Array<T> wrapper = JsonUtility.FromJson<Array<T>>(json);
            return wrapper.array;
        }

        public static string ToJson<T>(T[] array)
        {
            Array<T> wrapper = new Array<T>();
            wrapper.array = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Array<T> wrapper = new Array<T>();
            wrapper.array = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private struct Array<T>
        {
            public T[] array;
        }
    }
}
