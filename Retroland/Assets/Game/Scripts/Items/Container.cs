using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [System.Serializable]
    public class Container
    {
        [SerializeField] private List<ItemStack> _stacks;

        private List<ItemStack> Stacks { get => _stacks; }

        public void Add(ItemStack stack)
        {
            if (stack.IsEmpty)
            {
                return;
            }

            for (int i = 0; i < Stacks.Count; i++)
            {
                ItemStack tempStack = Stacks[i];

                if (!tempStack.IsFull && tempStack.item == stack.item)
                {
                    ItemStack newStack = tempStack;
                    newStack.quantity = Mathf.Min(tempStack.quantity + stack.quantity, stack.item.MaxStackSize);
                    stack.quantity -= newStack.quantity - tempStack.quantity;

                    if (stack.IsEmpty)
                    {
                        return;
                    }
                }
            }

            int stacksQuantity = Mathf.CeilToInt((float)stack.quantity / stack.item.MaxStackSize);

            for (int i = 0; i < stacksQuantity; i++)
            {
                Stacks.Add(new ItemStack(stack.item, Mathf.Min(stack.item.MaxStackSize, stack.quantity)));
                stack.quantity -= stack.item.MaxStackSize;
            }
        }

        public void Remove(ItemStack stack)
        {
            if (stack.IsEmpty)
            {
                return;
            }

            List<int> emptyIndex = new List<int>();

            for (int i = 0; i < Stacks.Count; i++)
            {
                ItemStack tempStack = Stacks[i];

                if (!tempStack.IsFull && tempStack.item == stack.item)
                {
                    ItemStack newStack = tempStack;
                    newStack.quantity = Mathf.Min(tempStack.quantity - stack.quantity, 0);
                    stack.quantity += newStack.quantity - tempStack.quantity;

                    if (newStack.IsEmpty)
                    {
                        emptyIndex.Add(i);
                    }

                    if (stack.IsEmpty)
                    {
                        break;
                    }
                }
            }

            foreach (int index in emptyIndex)
            {
                Stacks.RemoveAt(index);
            }
        }
    }
}
