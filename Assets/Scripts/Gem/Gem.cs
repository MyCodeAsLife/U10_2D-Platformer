using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gem : MonoBehaviour
{
    private readonly int ShineTrigger = Animator.StringToHash("isShine");

    private Animator _animator;
    private int _UILayer;

    public event Action OnPickup;

    private void Awake()
    {
        _UILayer = LayerMask.GetMask("UI");
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Shine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()            ////
    {
        OnPickup?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(_UILayer))
            OnPickup?.Invoke();
    }

    private IEnumerator Shine()
    {
        const float Second = 3f;
        var wait = new WaitForSeconds(Second);

        while (true)
        {
            _animator.SetTrigger(ShineTrigger);
            yield return wait;
        }
    }
}
