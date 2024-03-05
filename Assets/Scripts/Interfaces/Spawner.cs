using UnityEngine;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform _points;

        protected WaitForSeconds _delay;
        protected Transform[] _spawnPoints;

        private float _timeRespawn;


        private void Awake()
        {
            _spawnPoints = new Transform[_points.childCount];
            _timeRespawn = 2.5f;
            _delay = new WaitForSeconds(_timeRespawn);

            for (int i = 0; i < _spawnPoints.Length; i++)
                _spawnPoints[i] = _points.GetChild(i);
        }
    }
}