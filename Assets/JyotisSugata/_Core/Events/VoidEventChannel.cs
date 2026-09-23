using System;

using UnityEngine;

namespace JyotisSugata.Core.Events
{
    [CreateAssetMenu(menuName = "JyotisSugata/Events/Void Event Channel")]
    public class VoidEventChannel : ScriptableObject
    {
        private Action _onEventRaised;

        public void Raise()
        {
            _onEventRaised?.Invoke();
        }

        public void Subscribe(Action listener)
        {
            _onEventRaised += listener;
        }

        public void Unsubscribe(Action listener)
        {
            _onEventRaised -= listener;
        }
    }
}
