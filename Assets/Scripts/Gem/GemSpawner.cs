using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Gem _gem;

    private float _timeRespawn;

    private void Start()
    {
        _timeRespawn = 2.5f;
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_timeRespawn);
    }
}
