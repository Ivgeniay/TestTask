using System;

namespace Unitils
{
    internal class ObservedVariable<T>
    {
        public event Action<T> OnValueChangedEvent;
        private T _value;
        public T Value { 
            get => _value; 
            set {
                if (!_value.Equals(value)) {
                    _value = value;
                    OnValueChangedEvent?.Invoke(value);
                }
            }
        
        }
    }
}
