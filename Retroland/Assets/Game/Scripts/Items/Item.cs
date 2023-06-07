using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;
        [SerializeField][Min(1)] private int _maxStackSize = 1;

        public int MaxStackSize { get => _maxStackSize; }
        public string Description { get => _description; }
        public Sprite Icon { get => _icon; }

        public virtual void Use(Inventory inventory)
        {

        }
    }
}
