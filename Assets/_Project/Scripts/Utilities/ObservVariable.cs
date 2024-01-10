using System;
using System.Collections.Generic;

namespace Clock.Utilities
{
    internal class ObservVariable<T>
    { 
        public event Action<T> OnChangeEvent;
        private T value;
        public T Value { get => value;
            set
            {
                if (!value.Equals(this.value))
                {
                    this.value = value;
                    OnChangeEvent?.Invoke(this.value);
                }
            }
        }

        public static bool operator ==(ObservVariable<T> a, T b) =>
            a?.Value.Equals(b) ?? false;
        
        public static bool operator !=(ObservVariable<T> a, T b) =>
            !(a == b);
        
        public static ObservVariable<T> operator +(ObservVariable<T> a, Action<T> handler)
        {
            a.OnChangeEvent += handler;
            return a;
        } 
        public static ObservVariable<T> operator -(ObservVariable<T> a, Action<T> handler)
        {
            a.OnChangeEvent -= handler;
            return a;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObservVariable<T> other)
                return EqualityComparer<T>.Default.Equals(this.Value, other.Value); 
            return false;
        } 
        public override int GetHashCode() =>
            EqualityComparer<T>.Default.GetHashCode(this.Value);
        
    }
}
