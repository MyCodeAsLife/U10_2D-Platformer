using System.Collections;
using UnityEngine;

namespace Game
{
    public class ItemSpawner : MonoBehaviour
    {
        protected WaitForSeconds Delay;
        protected Transform[] SpawnPoints;

        [SerializeField] private Transform _points;
        [SerializeField] private PickUpItem _prefabItem;

        private float _timeRespawn;
        private PickUpItem _item;

        private void Awake()
        {
            SpawnPoints = new Transform[_points.childCount];
            _timeRespawn = 2.5f;
            Delay = new WaitForSeconds(_timeRespawn);

            for (int i = 0; i < SpawnPoints.Length; i++)
                SpawnPoints[i] = _points.GetChild(i);
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        public void OnPickedUp()
        {
            _item.PickedUp -= OnPickedUp;
            Destroy(_item.gameObject);
            StopCoroutine(Spawn());
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            yield return Delay;

            int randomIndex = Random.Range(0, SpawnPoints.Length);
            _item = Instantiate(_prefabItem, SpawnPoints[randomIndex].position, Quaternion.identity);
            _item.PickedUp += OnPickedUp;
        }
    }
}