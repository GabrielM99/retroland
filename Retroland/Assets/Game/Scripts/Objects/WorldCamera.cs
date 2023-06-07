using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class WorldCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        public Camera Base { get; set; }

        private float Speed { get => _speed; }
        private Transform Target { get => _target; set => _target = value; }
        private Vector3 TargetPosition => Target == null ? transform.position : new Vector3(Target.position.x, Target.position.y, transform.position.z);

        private void Awake()
        {
            Base = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            FollowTarget();
        }

        public void SetTarget(Transform target)
        {
            Target = target;
            Snap();
        }

        public void Snap()
        {
            transform.position = TargetPosition;
        }

        private void FollowTarget()
        {
            if (Target != null)
            {
                transform.position = Vector3.Lerp(transform.position, TargetPosition, Speed * Time.deltaTime);
            }
        }
    }
}
