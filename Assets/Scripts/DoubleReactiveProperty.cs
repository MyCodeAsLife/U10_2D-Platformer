using System;

namespace Game
{
    public class DoubleReactiveProperty<T1, T2>
    {
        public event Action<T1, T2> OnChanged;

        private T1 _value1;
        private T2 _value2;

        public T1 Value1
        {
            get => _value1;
            set
            {
                _value1 = value;
                OnChanged?.Invoke(_value1, _value2);
            }
        }

        public T2 Value2
        {
            get => _value2;
            set
            {
                _value2 = value;
                OnChanged?.Invoke(_value1, _value2);
            }
        }

        public void SetValues(T1 value1, T2 value2)
        {
            _value1 = value1;
            _value2 = value2;
            OnChanged?.Invoke(_value1, _value2);
        }
    }
}
