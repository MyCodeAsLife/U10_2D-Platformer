using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    //private const string Speed = "Speed";

    [SerializeField] private Transform _route;

    private Transform[] _waypoints;
    private Transform _targetPoint;
    //private Animator _animator;

    private int _nextPointIndex;
    private float _moveSpeed;
    private float _minDistance;

    private void Start()
    {
        _minDistance = 1.9f;
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
        Vector3 direction = (_targetPoint.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;
    }

    private void Move()
    {
        //_animator.SetFloat(Speed, _moveSpeed);

        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }

    private IEnumerator Movement()
    {
        const float Second = 5f;

        var wait = new WaitForSeconds(Second);

        while (true)
        {
            Move();
            //Rotate();

            float distance = Vector3.Distance(transform.position, _targetPoint.position);

            if (distance <= _minDistance)
            {
                //_animator.SetFloat(Speed, 0);

                yield return wait;

                //_animator.SetFloat(Speed, _moveSpeed);
            }

            yield return null;
        }
    }
}
