using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

namespace Game.Entities
{
    public class Player : Creature
    {
        [SerializeField] private TextMeshProUGUI _textMesh;

        [SyncVar] private CharacterData _characterData;

        public CharacterData CharacterData { get => _characterData; set => _characterData = value; }

        private Vector2 LastDirection { get; set; }
        private TextMeshProUGUI TextMesh { get => _textMesh; }

        protected override void Start()
        {
            base.Start();

            TextMesh.text = CharacterData.name;

            if (isLocalPlayer)
            {
                World.Camera.SetTarget(transform);
            }
        }

        protected override void Update()
        {
            base.Update();
            Move();
        }

        private void Move()
        {
            if (isLocalPlayer)
            {
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (direction != LastDirection)
                {
                    CmdMove(direction);
                    LastDirection = direction;
                }
            }
        }

        [Command]
        private void CmdMove(Vector2 direction)
        {
            this.Direction = direction;
        }
    }
}
