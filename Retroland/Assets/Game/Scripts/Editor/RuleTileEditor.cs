using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile;

namespace Game
{
    public class RuleTilePluginEditor : Editor
    {
        [MenuItem("Assets/Apply Default Collider")]
        private static void ApplyDefaultCollider()
        {
            RuleTile ruleTile = Selection.activeObject as RuleTile;

            foreach (TilingRule rule in ruleTile.m_TilingRules)
            {
                rule.m_ColliderType = ruleTile.m_DefaultColliderType;
            }
        }

        [MenuItem("Assets/Apply Default Collider", true)]
        private static bool ApplyDefaultColliderValidation()
        {
            return Selection.activeObject.GetType() == typeof(RuleTile);
        }
    }
}
