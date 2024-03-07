using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerObstacle;
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpPower;

        private PlayerInputActions _playerInputActions;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _groundCheckCollider;

        private Vector2 _newInputVector;
        private bool _onMoveHorizontal;
        private bool _onMoveVertical;
        private bool _onJump;
        private bool _isBlocksJump;
        private bool _isGrounded;
        private bool _flipX;

        public event Action<bool> OnGrounded;
        public event Action<bool> OnRunning;
        public event Action<bool> OnDirection;
        public event Action<bool, bool> OnAttack;
        private event Action MoveUpdate;

        private void Awake()
        {
            _onMoveVertical = false;
            _onMoveHorizontal = false;
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
            _playerInputActions.Disable();

            StopAllCoroutines();
        }

        private void LateUpdate()
        {
            GroundCheck();
        }

        private void Update()
        {
            MoveUpdate?.Invoke();
        }

        private float GetMovementHorizontalVector() => _playerInputActions.Movement.MoveHorizontal.ReadValue<float>();
        private float GetMovementVerticalVector() => _playerInputActions.Movement.MoveVertical.ReadValue<float>();

        private void MoveHorizontalEnable(InputAction.CallbackContext obj)
        {
            MoveHorizontalCalculate();

            if (_newInputVector.x < 0)
                _flipX = true;
            else if (_newInputVector.x > 0)
                _flipX = false;

            OnDirection?.Invoke(_flipX);

            if (_onMoveHorizontal == false)
            {
                _onMoveHorizontal = true;
                MoveUpdate += MoveHorizontal;
            }
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
            _onMoveHorizontal = false;
            MoveUpdate -= MoveHorizontal;

            OnRunning?.Invoke(_onMoveHorizontal);
        }

        private void MoveVerticalDisable(InputAction.CallbackContext obj)
        {
            _onMoveVertical = false;
            MoveUpdate -= MoveVertical;
        }

        private void GroundCheck()
        {
            _isGrounded = Physics2D.OverlapCapsule(_groundChecker.position, _groundCheckCollider.size, CapsuleDirection2D.Horizontal, 0, _layerObstacle);

            OnGrounded?.Invoke(_isGrounded);
        }

        private void MoveHorizontalCalculate()
        {
            _newInputVector.x = GetMovementHorizontalVector() * _moveSpeed;
        }

        private void MoveVerticalCalculate()
        {
            _newInputVector.y = GetMovementVerticalVector() * _moveSpeed;
        }

        private void MoveHorizontal()
        {
            OnRunning?.Invoke((int)_newInputVector.x != 0);
            _rigidbody.velocity = new Vector2(_newInputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
        }

        private void MoveVertical()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _newInputVector.y * _moveSpeed * Time.deltaTime);
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
            if (_isGrounded && _isBlocksJump == false)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                _rigidbody.AddForce(Vector2.up * _jumpPower);
                _isBlocksJump = true;

                StartCoroutine(UnlockJump());
            }
        }

        private void AttackEnable(InputAction.CallbackContext obj)
        {
            OnAttack?.Invoke(true, _flipX);
        }

        private void AttackDisable(InputAction.CallbackContext obj)
        {
            OnAttack?.Invoke(false, _flipX);
        }

        private IEnumerator UnlockJump()
        {
            float delay = 0.1f;
            yield return new WaitForSeconds(delay);
            _isBlocksJump = false;
        }
    }
}