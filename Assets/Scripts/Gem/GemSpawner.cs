using System.Collections;
using UnityEngine;

namespace Game
{
    public class GemSpawner : Spawner
    {
        [SerializeField] private Gem _prefab;

        private Gem _gem;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        public void OnPickedUp()
        {
            _gem.PickedUp -= OnPickedUp;
            Destroy(_gem.gameObject);
            StopCoroutine(Spawn());
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return _delay;

            int randomIndex = Random.Range(0, _spawnPoints.Length);
            _gem = Instantiate(_prefab, _spawnPoints[randomIndex].position, Quaternion.identity);
            _gem.PickedUp += OnPickedUp;
        }
    }
}