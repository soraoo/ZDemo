using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;

namespace ZXC
{
    public delegate void OnValueChangedHandler<T>(T oldValue, T newValue);

    public class PropertyBase<T> : IZProperty<T>
    {
        public event OnValueChangedHandler<T> ValueChangedChangeEvent;

        private T val;

        public T Value
        {
            get
            {
                return val;
            }
            set
            {
                if(val.Equals(value))
                    return;
                ValueChangedChangeEvent?.Invoke(val, value);
                val = value;
            }
        }
    }
}