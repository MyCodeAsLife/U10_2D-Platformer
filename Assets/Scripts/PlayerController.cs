using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _rigidbody;

    private Vector2 _inputVector;
    private int _layerObstacle;

    private float _rayCastDistance;
    private float _moveSpeed;
    private float _jumpPower;

    private bool _onMove;
    private bool _onJump;
    private bool _isBlocksJump;
    private bool _isGrounded;

    private event Action MoveUpdate;

    private void Awake()
    {
        _onMove = false;
        _onJump = false;
        _isBlocksJump = false;

        _moveSpeed = 15f;
        _jumpPower = 650f;
        _layerObstacle = 64;                                // Слой номер 6

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _playerInputActions = new PlayerInputActions();

        float _offset = 0.01f;
        float _half = 0.5f;
        _rayCastDistance = transform.localScale.y * _collider.size.y * _half + _offset;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
        _playerInputActions.Movement.Move.performed += MoveEnable;
        _playerInputActions.Movement.Move.canceled += MoveDisable;
        _playerInputActions.Movement.Jump.performed += JumpEnable;
        _playerInputActions.Movement.Jump.canceled += JumpDisable;
    }

    private void OnDisable()
    {
        _playerInputActions.Movement.Move.performed -= MoveEnable;
        _playerInputActions.Movement.Move.canceled -= MoveDisable;
        _playerInputActions.Movement.Jump.performed -= JumpEnable;
        _playerInputActions.Movement.Jump.canceled -= JumpDisable;
        _playerInputActions.Disable();

        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        _isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, _rayCastDistance, _layerObstacle);
        Debug.DrawRay(transform.position, -Vector2.up * _rayCastDistance, Color.green);
    }

    private void Update()
    {
        MoveUpdate?.Invoke();
    }

    private Vector2 GetMovementVector() => _playerInputActions.Movement.Move.ReadValue<Vector2>();

    private void MoveEnable(InputAction.CallbackContext obj)
    {
        MoveCalculate();

        if (_onMove == false)
        {
            MoveUpdate += Move;
            _onMove = true;
        }
    }

    private void MoveDisable(InputAction.CallbackContext obj)
    {
        //_rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);     // Для отключение инерции
        MoveUpdate -= Move;
        _onMove = false;
    }

    private void MoveCalculate()
    {
        _inputVector = GetMovementVector().normalized * _moveSpeed;
    }

    private void Move()
    {
        _rigidbody.velocity = new Vector2(_inputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
        _rigidbody.velocity = new Vector2(_inputVector.x * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
    }

    private void JumpEnable(InputAction.CallbackContext obj)
    {
        if (_onJump == false)
        {
            MoveUpdate += Jump;
            _onJump = true;
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
            _rigidbody.AddForce(Vector2.up * _jumpPower);
            _isBlocksJump = true;
            StartCoroutine(UnlockJump());
        }
    }

    private IEnumerator UnlockJump()
    {
        float delay = 0.06f;
        yield return new WaitForSeconds(delay);
        _isBlocksJump = false;
    }
}
