using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _route;

    private Transform[] _waypoints;
    private Transform _targetPoint;

    private bool _isLeft;
    private int _nextPointIndex;
    private float _moveSpeed;
    private float _minDistance;
    private float _newPosX;

    public event Action<bool> OnRunning;
    public event Action<bool> OnDirection;

    private void Start()
    {
        _isLeft = false;
        _minDistance = 1.3f;
        _moveSpeed = 3;
        _waypoints = new Transform[_route.childCount];

        for (int i = 0; i < _waypoints.Length; i++)
            _waypoints[i] = _route.GetChild(i);

        _targetPoint = _waypoints[_nextPointIndex];
        StartCoroutine(Movement());
    }

    private void Move()
    {
        _newPosX = Mathf.MoveTowards(transform.position.x, _targetPoint.position.x, _moveSpeed * Time.deltaTime);

        if (_newPosX - transform.position.x < 0)
            _isLeft = false;
        else
            _isLeft = true;

        OnDirection?.Invoke(_isLeft);
        transform.position = new Vector2(_newPosX, transform.position.y);
    }

    private IEnumerator Movement()
    {
        const float Second = 3.5f;
        bool isMove = true;
        var wait = new WaitForSeconds(Second);


        while (isMove)
        {
            Move();
            OnRunning?.Invoke(true);

            float distance = Vector2.Distance(transform.position, _targetPoint.position);

            if (distance <= _minDistance)
            {
                OnRunning?.Invoke(false);
                _nextPointIndex = (_nextPointIndex + 1) % _waypoints.Length;
                _targetPoint = _waypoints[_nextPointIndex];
                yield return wait;
            }

            yield return null;
        }
    }
}