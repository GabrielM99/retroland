using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class Entity : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _obstacleLayers;

        public Vector2 Direction { get; set; }

        private float Speed { get => _speed; }
        private LayerMask ObstacleLayers { get => _obstacleLayers; }
        private Vector2 TargetPosition { get; set; }

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            TargetPosition = World.WorldToTilePosition(transform.position);
        }

        protected virtual void Update()
        {
            Move();
        }

        private void Move()
        {
            if (isServer)
            {
                Vector2 direction = this.Direction;

                direction.x = MathHelper.DeviateToInt(direction.x);
                direction.y = MathHelper.DeviateToInt(direction.y);

                if (direction != Vector2.zero)
                {
                    if (Mathf.Approximately(((Vector2)transform.position - TargetPosition).sqrMagnitude, 0))
                    {
                        Vector2 targetPosition = World.WorldToTilePosition((Vector2)transform.position + direction);

                        if (CanMove(targetPosition))
                        {
                            this.TargetPosition = targetPosition;
                        }
                    }
                }

                transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
            }
        }

        private bool CanMove(Vector2 position)
        {
            return !Physics2D.OverlapPoint(position, ObstacleLayers);
        }
    }
}
