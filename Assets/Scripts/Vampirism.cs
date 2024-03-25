using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Vampirism : ISkill
{
    [SerializeField] private ContactFilter2D _contactFilter;

    private float _radius = 3f;

    public override event Action<IDamageble> OnHit;

    private void OnEnable()
    {
        List<Collider2D> hits = new List<Collider2D>();
        //Physics2D.OverlapBox(transform.position, _radius, 0, _contactFilter, hits);
        Physics2D.OverlapCircle(transform.position, _radius, _contactFilter, hits);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<IDamageble>(out IDamageble obj))
                OnHit?.Invoke(obj);
        }
    }
}
