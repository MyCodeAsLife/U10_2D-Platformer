using System;

namespace Game
{
    public class SingleReactiveProperty<T>
    {
        private T _value;

        public event Action<T> Changed;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(_value);
            }
        }
    }
}
