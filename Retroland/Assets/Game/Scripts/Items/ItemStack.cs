using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [System.Serializable]
    public struct ItemStack
    {
        public Item item;
        public int quantity;
        public bool IsEmpty => item.Equals(default(Item)) || quantity <= 0;
        public bool IsFull => !IsEmpty && quantity >= item.MaxStackSize;

        public ItemStack(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}
