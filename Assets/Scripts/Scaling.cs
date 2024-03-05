using UnityEngine;

namespace Game
{
    public class Scaling : MonoBehaviour
    {
        private float _speed;
        private float _maxSize;
        private float _minSize;

        private void Awake()
        {
            _speed = 0.005f;
            _maxSize = 1.1f;
            _minSize = 0.9f;
        }

        void LateUpdate()
        {
            if ((transform.localScale.y > _maxSize && _speed > 0) || (transform.localScale.y < _minSize && _speed < 0))
                _speed *= -1;

            transform.localScale = (transform.localScale * (1 + _speed));
        }
    }
}