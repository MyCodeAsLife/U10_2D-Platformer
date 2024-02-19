using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    [SerializeField] private Transform _groundCheker;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundCheckDistance = 0.4f;
    [SerializeField] private float _jumpHeight;

    private CharacterController _characterController;
    private float _velocity;
    private bool _isGrounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundCheker.position, _groundCheckDistance, _groundMask);

        if (_isGrounded && _velocity < 0f)
        {
            _velocity = -2f;
        }

        Move();
        DoGravity();
    }

    private void Update()
    {
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), 0f);
        _characterController.Move(direction * (_speed * Time.deltaTime));
    }

    private void Jump()
    {
        _velocity = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }

    private void DoGravity()
    {
        _velocity += _gravity * Time.fixedDeltaTime;
        _characterController.Move(new Vector2(0f, _velocity * Time.fixedDeltaTime));
    }
}
