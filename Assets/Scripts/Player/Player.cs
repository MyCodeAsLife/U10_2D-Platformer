using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : Character
    {
        private PlayerController _controller;
        private Vampirism _vampirism;
        private Coroutine _pumpOver;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<PlayerController>();
            _vampirism = this.gameObject.AddComponent<Vampirism>();
            //GetComponent<Vampirism>();
            //_vampirism = new Vampirism();

        }

        private void OnEnable()
        {
            _controller.OnAttack += ChangeAttackState;
            _controller.OnDirection += ChangeDirection;
        }

        private void OnDisable()
        {
            _controller.OnAttack -= ChangeAttackState;
            _controller.OnDirection -= ChangeDirection;
        }
    }
}
