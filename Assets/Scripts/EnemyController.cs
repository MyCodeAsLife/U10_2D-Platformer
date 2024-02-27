using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    //private const string Speed = "Speed";

    [SerializeField] private Transform _route;

    private SpriteRenderer _spriteRenderer;
    private Transform[] _waypoints;
    private Transform _targetPoint;
    //private Animator _animator;

    private int _nextPointIndex;
    private float _moveSpeed;
    private float _minDistance;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _minDistance = 1.3f;
        _moveSpeed = 3;
        _waypoints = new Transform[_route.childCount];

        for (int i = 0; i < _waypoints.Length; i++)
            _waypoints[i] = _route.GetChild(i);

        _targetPoint = _waypoints[_nextPointIndex];
        //_animator = GetComponent<Animator>();

        StartCoroutine(Movement());
    }

    private void Rotate()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    private void Move()
    {
        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, _targetPoint.position.x, _moveSpeed * Time.deltaTime), transform.position.y);
    }

    private IEnumerator Movement()
    {
        const float Second = 3.5f;
        bool isMove = true;

        var wait = new WaitForSeconds(Second);

        while (isMove)
        {
            Move();
            Rotate();       // ƒобавить условие дл€ поворота(доп тригер?). ≈сли скорость > 0 то идем в право, иначе в лево.

            float distance = Vector2.Distance(transform.position, _targetPoint.position);

            if (distance <= _minDistance)
            {
                //_animator.SetFloat(Speed, 0);

                _nextPointIndex = (_nextPointIndex + 1) % _waypoints.Length;
                _targetPoint = _waypoints[_nextPointIndex];
                yield return wait;

                //_animator.SetFloat(Speed, _moveSpeed);
            }
            yield return null;
        }
    }
}
