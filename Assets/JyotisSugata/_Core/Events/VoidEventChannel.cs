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
