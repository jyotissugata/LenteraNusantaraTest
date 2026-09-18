using JyotisSugata.Core.Events;
using JyotisSugata.Core.StateMachine;
using JyotisSugata.Core.Input;
using JyotisSugata.Exploration.Player;
using JyotisSugata.Exploration.Interaction;
using JyotisSugata.Puzzles.Shared;
using JyotisSugata.Puzzles.MemoryMatch;
using JyotisSugata.Puzzles.NumpadPasscode;
using JyotisSugata.UI.HUD;
using JyotisSugata.UI.Transitions;

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
