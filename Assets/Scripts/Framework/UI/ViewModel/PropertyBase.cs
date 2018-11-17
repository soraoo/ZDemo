using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;

namespace ZXC
{
    public delegate void OnValueChangedHandler<T>(T oldValue, T newValue);

    public class PropertyBase<T> : IZProperty<T>
    {
        public event OnValueChangedHandler<T> valueChangeEvent;

        private T val;

        public T Value
        {
            get
            {
                return val;
            }
            set
            {
                if(!val.Equals(value))
                {
                    if (valueChangeEvent != null)
                    {
                        valueChangeEvent(val, value);
                    }
                    val = value;
                }
            }
        }
    }
}