using System.Collections;
using UnityEngine;

namespace Game
{
    public class AidkitSpawner : Spawner
    {
        [SerializeField] private Aidkit _prefab;

        private Aidkit _aidkit;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        public void OnPickedUp()
        {
            _aidkit.PickedUp -= OnPickedUp;
            Destroy(_aidkit.gameObject);
            StopAllCoroutines();
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return _delay;

            int randomIndex = Random.Range(0, _spawnPoints.Length);
            _aidkit = Instantiate(_prefab, _spawnPoints[randomIndex].position, Quaternion.identity);
            _aidkit.PickedUp += OnPickedUp;
        }
    }
}