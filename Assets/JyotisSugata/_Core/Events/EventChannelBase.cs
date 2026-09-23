using System;

using UnityEngine;

namespace JyotisSugata.Core.Events
{
    public abstract class EventChannelBase<T> : ScriptableObject
    {
        private Action<T> _onEventRaised;

        public void Raise(T value)
        {
            _onEventRaised?.Invoke(value);
        }

        public void Subscribe(Action<T> listener)
        {
            _onEventRaised += listener;
        }

        public void Unsubscribe(Action<T> listener)
        {
            _onEventRaised -= listener;
        }
    }
}
