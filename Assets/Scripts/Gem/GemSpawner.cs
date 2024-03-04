using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Gem _prefab;
    [SerializeField] private Transform _points;

    private Transform[] _spawnPoints;
    private WaitForSeconds _delay;
    private Gem _gem;
    private float _timeRespawn;


    private void Start()
    {
        _spawnPoints = new Transform[_points.childCount];
        _timeRespawn = 2.5f;
        _delay = new WaitForSeconds(_timeRespawn);

        for (int i = 0; i < _spawnPoints.Length; i++)
            _spawnPoints[i] = _points.GetChild(i);

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
        yield return _delay;

        int randomIndex = Random.Range(0, _spawnPoints.Length);
        _gem = Instantiate(_prefab, _spawnPoints[randomIndex].position, Quaternion.identity);
        _gem.OnPickup += GemPickup;
    }
}
