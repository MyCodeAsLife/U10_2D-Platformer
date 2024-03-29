using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public readonly SingleReactiveProperty<bool> IsGrounded = new();
        public readonly SingleReactiveProperty<bool> IsRunning = new();
        public readonly SingleReactiveProperty<bool> IsDirectionLeft = new();
        public readonly DoubleReactiveProperty<bool, SkillEnum> IsSkillUsed = new();

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

        public event Action MovementUpdate;

        private void Awake()
        {
            _onMoveVertical = false;
            IsRunning.Value = false;
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
            _playerInputActions.Movement.Jump.performed += OnEnableJump;
            _playerInputActions.Movement.Jump.canceled += OnDisableJump;
            _playerInputActions.Movement.MoveHorizontal.performed += OnEnableHorizontalMovement;
            _playerInputActions.Movement.MoveVertical.performed += OnEnableVerticalMovement;
            _playerInputActions.Movement.MoveHorizontal.canceled += OnDisableHorizontalMovement;
            _playerInputActions.Movement.MoveVertical.canceled += OnDisableVerticalMovement;
            _playerInputActions.Movement.Attack.performed += OnAttackEnable;
            _playerInputActions.Movement.Attack.canceled += OnAttackDisable;
            _playerInputActions.Movement.Vampirism.performed += OnVampirismEnable;
            _playerInputActions.Movement.Vampirism.canceled += OnVampirismDisable;
        }

        private void OnDisable()
        {
            _playerInputActions.Movement.Jump.performed -= OnEnableJump;
            _playerInputActions.Movement.Jump.canceled -= OnDisableJump;
            _playerInputActions.Movement.MoveHorizontal.performed -= OnEnableHorizontalMovement;
            _playerInputActions.Movement.MoveVertical.performed -= OnEnableVerticalMovement;
            _playerInputActions.Movement.MoveHorizontal.canceled -= OnDisableHorizontalMovement;
            _playerInputActions.Movement.MoveVertical.canceled -= OnDisableVerticalMovement;
            _playerInputActions.Movement.Attack.performed -= OnAttackEnable;
            _playerInputActions.Movement.Attack.canceled -= OnAttackDisable;
            _playerInputActions.Movement.Vampirism.performed -= OnVampirismEnable;
            _playerInputActions.Movement.Vampirism.canceled -= OnVampirismDisable;
            _playerInputActions.Disable();
        }

        private void FixedUpdate()
        {
            GroundCheck();
            MovementUpdate?.Invoke();
        }

        private float GetMovementHorizontalVector() => _playerInputActions.Movement.MoveHorizontal.ReadValue<float>();
        private float GetMovementVerticalVector() => _playerInputActions.Movement.MoveVertical.ReadValue<float>();

        private void OnEnableHorizontalMovement(InputAction.CallbackContext obj)
        {
            CalculateHorizontalMovement();

            if (_InputVector.x < 0)
                IsDirectionLeft.Value = true;
            else if (_InputVector.x > 0)
                IsDirectionLeft.Value = false;

            if (IsRunning.Value == false)
                MovementUpdate += OnHorizontalMovement;
        }

        private void OnEnableVerticalMovement(InputAction.CallbackContext obj)
        {
            CalculateVerticalMovement();

            if (_onMoveVertical == false)
            {
                _onMoveVertical = true;
                MovementUpdate += OnVerticalMovement;
            }
        }

        private void OnDisableHorizontalMovement(InputAction.CallbackContext obj)
        {
            IsRunning.Value = false;
            MovementUpdate -= OnHorizontalMovement;
        }

        private void OnDisableVerticalMovement(InputAction.CallbackContext obj)
        {
            _onMoveVertical = false;
            MovementUpdate -= OnVerticalMovement;
        }

        private void GroundCheck()
        {
            IsGrounded.Value = Physics2D.OverlapCapsule(_groundChecker.position, _groundCheckCollider.size, CapsuleDirection2D.Horizontal, 0, _layerObstacle);
        }

        private void CalculateHorizontalMovement()
        {
            _InputVector.x = GetMovementHorizontalVector() * _moveSpeed;
        }

        private void CalculateVerticalMovement()
        {
            _InputVector.y = GetMovementVerticalVector() * _moveSpeed;
        }

        private void OnHorizontalMovement()
        {
            IsRunning.Value = (int)_InputVector.x != 0;
            _rigidbody.velocity = new Vector2(_InputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
        }

        private void OnVerticalMovement()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _InputVector.y * _moveSpeed * Time.deltaTime);
        }

        private void OnEnableJump(InputAction.CallbackContext obj)
        {
            if (_onJump == false)
            {
                _onJump = true;
                MovementUpdate += OnJumped;
            }
        }

        private void OnDisableJump(InputAction.CallbackContext obj)
        {
            MovementUpdate -= OnJumped;
            _onJump = false;
        }

        private void OnJumped()
        {
            if (IsGrounded.Value && _isBlocksJump == false)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                _rigidbody.AddForce(Vector2.up * _jumpPower);
                _isBlocksJump = true;

                StartCoroutine(UnlockJump());
            }
        }

        private void OnAttackEnable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(true, SkillEnum.Slash);
        }

        private void OnAttackDisable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(false, SkillEnum.Slash);
        }

        private void OnVampirismEnable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(true, SkillEnum.Vampirism);
        }

        private void OnVampirismDisable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(false, SkillEnum.Vampirism);
        }

        private IEnumerator UnlockJump()
        {
            float delay = 0.1f;
            yield return new WaitForSeconds(delay);
            _isBlocksJump = false;
        }
    }
}