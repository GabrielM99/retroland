using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.Items
{
    [RequireComponent(typeof(Creature))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Container _backpack;
        [SerializeField] private Container _equipments;

        public Container Backpack { get => _backpack; }
        public Container Equipments { get => _equipments; }
        public Creature Creature { get; private set; }

        private void Awake()
        {
            Creature = GetComponent<Creature>();
        }
    }
}
