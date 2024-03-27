using System;

namespace Game
{
    public class SingleReactiveProperty<T>
    {
        public event Action<T> OnChanged;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }
    }
}
