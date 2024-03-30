using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class GroundChecker : MonoBehaviour
    {
        public readonly SingleReactiveProperty<bool> IsGrounded = new();

        private LayerMask _layerObstacle;
        private CapsuleCollider2D _groundCheckCollider;

        private void Awake()
        {
            _layerObstacle = 64;
            IsGrounded.Value = false;

            if (TryGetComponent<CapsuleCollider2D>(out _groundCheckCollider) == false)
                throw new NullReferenceException(nameof(CapsuleCollider2D));

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TilemapCollider2D>(out TilemapCollider2D ground))
                IsGrounded.Value = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TilemapCollider2D>(out TilemapCollider2D ground))
                IsGrounded.Value = false;
        }
    }
}