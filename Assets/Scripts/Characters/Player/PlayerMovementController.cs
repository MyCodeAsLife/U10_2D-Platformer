using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    public readonly SingleReactiveProperty<bool> IsRunning = new();
    public readonly SingleReactiveProperty<bool> IsDirectionLeft = new();

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    private GroundChecker _groundChecker;
    private PlayerInputController _playerController;
    private Rigidbody2D _rigidbody;

    private Vector2 _InputVector;
    private bool _onMoveVertical;
    private bool _onJump;
    private bool _isBlocksJump;

    public event Action MovementUpdate;

    public SingleReactiveProperty<bool> IsGrounded { get { return _groundChecker.IsGrounded; } }

    private void Awake()
    {
        _onMoveVertical = false;
        IsRunning.Value = false;
        _onJump = false;
        _isBlocksJump = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerInputController>();
        _groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void FixedUpdate()
    {
        MovementUpdate?.Invoke();
    }

    public void OnEnableHorizontalMovement(InputAction.CallbackContext obj)
    {
        CalculateHorizontalMovement();

        if (_InputVector.x < 0)
            IsDirectionLeft.Value = true;
        else if (_InputVector.x > 0)
            IsDirectionLeft.Value = false;

        if (IsRunning.Value == false)
            MovementUpdate += OnHorizontalMovement;
    }

    public void OnEnableVerticalMovement(InputAction.CallbackContext obj)
    {
        CalculateVerticalMovement();

        if (_onMoveVertical == false)
        {
            _onMoveVertical = true;
            MovementUpdate += OnVerticalMovement;
        }
    }

    public void OnDisableHorizontalMovement(InputAction.CallbackContext obj)
    {
        IsRunning.Value = false;
        MovementUpdate -= OnHorizontalMovement;
    }

    public void OnDisableVerticalMovement(InputAction.CallbackContext obj)
    {
        _onMoveVertical = false;
        MovementUpdate -= OnVerticalMovement;
    }

    public void OnHorizontalMovement()
    {
        IsRunning.Value = (int)_InputVector.x != 0;
        _rigidbody.velocity = new Vector2(_InputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
    }

    public void OnVerticalMovement()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _InputVector.y * _moveSpeed * Time.deltaTime);
    }

    public void OnEnableJump(InputAction.CallbackContext obj)
    {
        if (_onJump == false)
        {
            _onJump = true;
            MovementUpdate += OnJumped;
        }
    }

    public void OnDisableJump(InputAction.CallbackContext obj)
    {
        MovementUpdate -= OnJumped;
        _onJump = false;
    }

    public void OnJumped()
    {
        if (_groundChecker.IsGrounded.Value && _isBlocksJump == false)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(Vector2.up * _jumpPower);
            _isBlocksJump = true;

            StartCoroutine(UnlockJump());
        }
    }

    private float GetHorizontalMovementVector() => _playerController.UserInputActions.Movement.MoveHorizontal.ReadValue<float>();
    private float GetVerticalMovementVector() => _playerController.UserInputActions.Movement.MoveVertical.ReadValue<float>();

    private void CalculateHorizontalMovement()
    {
        _InputVector.x = GetHorizontalMovementVector() * _moveSpeed;
    }

    private void CalculateVerticalMovement()
    {
        _InputVector.y = GetVerticalMovementVector() * _moveSpeed;
    }

    private IEnumerator UnlockJump()
    {
        float delay = 0.1f;
        yield return new WaitForSeconds(delay);
        _isBlocksJump = false;
    }
}