using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : Character
    {
        private PlayerController _controller;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _controller.OnAttack += AttackChange;
        }

        private void OnDisable()
        {
            _controller.OnAttack -= AttackChange;
        }
    }
}
