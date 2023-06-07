using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MathHelper : MonoBehaviour
    {
        public static float DeviateToInt(float value)
        {
            return value > 0 ? Mathf.CeilToInt(value) : Mathf.FloorToInt(value);
        }

        public static Vector2 DeviateToInt(Vector2 vector)
        {
            return new Vector2(DeviateToInt(vector.x), DeviateToInt(vector.y));
        }
    }
}
