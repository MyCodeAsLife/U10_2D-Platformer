using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public readonly SingleReactiveProperty<bool> PROPERTY_GROUNDED = new();
        public readonly SingleReactiveProperty<bool> PROPERTY_RUNNING = new();
        public readonly SingleReactiveProperty<bool> PROPERTY_DIRECTION = new();
        public readonly DoubleReactiveProperty<bool, SkillEnum> PROPERTY_SKILL_USED = new();

        [SerializeField] private LayerMask _layerObstacle;
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpPower;

        private PlayerInputActions _playerInputActions;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _groundCheckCollider;

        private Vector2 _InputVector;
        private bool _onMoveVertical;
        private bool _onJump;
        private bool _isBlocksJump;

        public event Action MoveUpdate;

        private void Awake()
        {
            _onMoveVertical = false;
            PROPERTY_RUNNING.Value = false;
            _onJump = false;
            _isBlocksJump = false;
            _layerObstacle = 64;

            _rigidbody = GetComponent<Rigidbody2D>();
            _playerInputActions = new PlayerInputActions();

            if (_groundChecker.TryGetComponent<CapsuleCollider2D>(out _groundCheckCollider) == false)
                throw new NullReferenceException(nameof(CapsuleCollider2D));
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();
            _playerInputActions.Movement.Jump.performed += JumpEnable;
            _playerInputActions.Movement.Jump.canceled += JumpDisable;
            _playerInputActions.Movement.MoveHorizontal.performed += MoveHorizontalEnable;
            _playerInputActions.Movement.MoveVertical.performed += MoveVerticalEnable;
            _playerInputActions.Movement.MoveHorizontal.canceled += MoveHorizontalDisable;
            _playerInputActions.Movement.MoveVertical.canceled += MoveVerticalDisable;
            _playerInputActions.Movement.Attack.performed += AttackEnable;
            _playerInputActions.Movement.Attack.canceled += AttackDisable;
            _playerInputActions.Movement.Vampirism.performed += VampirismEnable;
            _playerInputActions.Movement.Vampirism.canceled += VampirismDisable;
        }

        private void OnDisable()
        {
            _playerInputActions.Movement.Jump.performed -= JumpEnable;
            _playerInputActions.Movement.Jump.canceled -= JumpDisable;
            _playerInputActions.Movement.MoveHorizontal.performed -= MoveHorizontalEnable;
            _playerInputActions.Movement.MoveVertical.performed -= MoveVerticalEnable;
            _playerInputActions.Movement.MoveHorizontal.canceled -= MoveHorizontalDisable;
            _playerInputActions.Movement.MoveVertical.canceled -= MoveVerticalDisable;
            _playerInputActions.Movement.Attack.performed -= AttackEnable;
            _playerInputActions.Movement.Attack.canceled -= AttackDisable;
            _playerInputActions.Movement.Vampirism.performed -= VampirismEnable;
            _playerInputActions.Movement.Vampirism.canceled -= VampirismDisable;
            _playerInputActions.Disable();

            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            GroundCheck();
            MoveUpdate?.Invoke();
        }

        private float GetMovementHorizontalVector() => _playerInputActions.Movement.MoveHorizontal.ReadValue<float>();
        private float GetMovementVerticalVector() => _playerInputActions.Movement.MoveVertical.ReadValue<float>();

        private void MoveHorizontalEnable(InputAction.CallbackContext obj)
        {
            MoveHorizontalCalculate();

            if (_InputVector.x < 0)
                PROPERTY_DIRECTION.Value = true;
            else if (_InputVector.x > 0)
                PROPERTY_DIRECTION.Value = false;

            if (PROPERTY_RUNNING.Value == false)
                MoveUpdate += MoveHorizontal;
        }

        private void MoveVerticalEnable(InputAction.CallbackContext obj)
        {
            MoveVerticalCalculate();

            if (_onMoveVertical == false)
            {
                _onMoveVertical = true;
                MoveUpdate += MoveVertical;
            }
        }

        private void MoveHorizontalDisable(InputAction.CallbackContext obj)
        {
            PROPERTY_RUNNING.Value = false;
            MoveUpdate -= MoveHorizontal;
        }

        private void MoveVerticalDisable(InputAction.CallbackContext obj)
        {
            _onMoveVertical = false;
            MoveUpdate -= MoveVertical;
        }

        private void GroundCheck()
        {
            PROPERTY_GROUNDED.Value = Physics2D.OverlapCapsule(_groundChecker.position, _groundCheckCollider.size, CapsuleDirection2D.Horizontal, 0, _layerObstacle);
        }

        private void MoveHorizontalCalculate()
        {
            _InputVector.x = GetMovementHorizontalVector() * _moveSpeed;
        }

        private void MoveVerticalCalculate()
        {
            _InputVector.y = GetMovementVerticalVector() * _moveSpeed;
        }

        private void MoveHorizontal()
        {
            PROPERTY_RUNNING.Value = (int)_InputVector.x != 0;
            _rigidbody.velocity = new Vector2(_InputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
        }

        private void MoveVertical()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _InputVector.y * _moveSpeed * Time.deltaTime);
        }

        private void JumpEnable(InputAction.CallbackContext obj)
        {
            if (_onJump == false)
            {
                _onJump = true;
                MoveUpdate += Jump;
            }
        }

        private void JumpDisable(InputAction.CallbackContext obj)
        {
            MoveUpdate -= Jump;
            _onJump = false;
        }

        private void Jump()
        {
            if (PROPERTY_GROUNDED.Value && _isBlocksJump == false)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                _rigidbody.AddForce(Vector2.up * _jumpPower);
                _isBlocksJump = true;

                StartCoroutine(UnlockJump());
            }
        }

        private void AttackEnable(InputAction.CallbackContext obj)
        {
            PROPERTY_SKILL_USED.SetValues(true, SkillEnum.Slash);
        }

        private void AttackDisable(InputAction.CallbackContext obj)
        {
            PROPERTY_SKILL_USED.SetValues(false, SkillEnum.Slash);
        }

        private void VampirismEnable(InputAction.CallbackContext obj)
        {
            PROPERTY_SKILL_USED.SetValues(true, SkillEnum.Vampirism);
        }

        private void VampirismDisable(InputAction.CallbackContext obj)
        {
            PROPERTY_SKILL_USED.SetValues(false, SkillEnum.Vampirism);
        }

        private IEnumerator UnlockJump()
        {
            float delay = 0.1f;
            yield return new WaitForSeconds(delay);
            _isBlocksJump = false;
        }
    }
}