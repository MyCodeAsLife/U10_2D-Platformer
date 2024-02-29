using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gem : MonoBehaviour
{
    private readonly int ShineTrigger = Animator.StringToHash("isShine");

    private Animator _animator;
    //private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
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
