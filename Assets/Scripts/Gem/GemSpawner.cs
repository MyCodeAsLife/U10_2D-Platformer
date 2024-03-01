using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Gem _prefab;
    [SerializeField] private Transform _points;

    private Gem _gem;
    private Transform[] _spawnPoints;
    private float _timeRespawn;


    private void Start()
    {
        _spawnPoints = new Transform[_points.childCount];

        for (int i = 0; i < _spawnPoints.Length; i++)
            _spawnPoints[i] = _points.GetChild(i);

        _timeRespawn = 2.5f;
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        _gem.OnPickup -= GemPickup;
    }

    public void GemPickup()
    {
        _gem.OnPickup -= GemPickup;
        Destroy(_gem.gameObject);
        StopAllCoroutines();
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_timeRespawn);

        int randomIndex = Random.Range(0, _spawnPoints.Length);
        _gem = Instantiate(_prefab, _spawnPoints[randomIndex].position, Quaternion.identity);
        _gem.OnPickup += GemPickup;
    }
}
