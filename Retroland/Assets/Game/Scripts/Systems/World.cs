using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using Mirror;
using UnityEngine;

namespace Game
{
    public class World : Singleton<World>
    {
        [SerializeField] private WorldCamera _camera;

        public static WorldCamera Camera { get => Instance._camera; }

        public static Vector2 WorldToTilePosition(Vector2 position)
        {
            return Vector2Int.FloorToInt(position);
        }
    }
}
