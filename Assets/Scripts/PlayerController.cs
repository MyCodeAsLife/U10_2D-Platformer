using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    private Vector2 _inputVector;
    private float _moveSpeed;
    private float _jumpPower;
    private float _offset;
    private int _layerObstacle;
    private bool _isMove;
    private bool _isGrounded;

    private event Action MoveUpdate;

    private void Awake()
    {
        _offset = 1000f;
        _moveSpeed = 15f;
        _jumpPower = 900f;
        _isMove = false;
        _layerObstacle = 32;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
        _playerInputActions.Movement.Move.performed += MoveEnable;
        _playerInputActions.Movement.Move.canceled += MoveDisable;
        _playerInputActions.Movement.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        _playerInputActions.Movement.Move.performed -= MoveEnable;
        _playerInputActions.Movement.Move.canceled -= MoveDisable;
        _playerInputActions.Movement.Jump.performed -= Jump;
        _playerInputActions.Disable();
    }

    private void LateUpdate()
    {
        _isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, _collider.size.y + _offset, _layerObstacle);   ////
        Debug.DrawRay(transform.position, -Vector2.up, Color.green, _collider.size.y + _offset);                        ////
        Debug.Log(_isGrounded);                                                                                         ////
    }

    private void Update()
    {
        MoveUpdate?.Invoke();
    }

    private Vector2 GetMovementVector() => _playerInputActions.Movement.Move.ReadValue<Vector2>();

    private void MoveEnable(InputAction.CallbackContext obj)
    {
        MoveCalculate();

        if (_isMove == false)
        {
            MoveUpdate += Move;
            _isMove = true;
        }
    }

    private void MoveDisable(InputAction.CallbackContext obj)
    {
        //_rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);     // Для отключение инерции
        MoveUpdate -= Move;
        _isMove = false;
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

    private void Jump(InputAction.CallbackContext obj)
    {
        Debug.Log("Jump");
        //if(_rigidbody.)
        //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
        _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, _jumpPower));
    }
}
