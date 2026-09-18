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

using UnityEngine;

namespace JyotisSugata.Core.Input
{
    public class InputReaderInitializer : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        private void Awake()
        {
            if (_inputReader != null)
            {
                _inputReader.Initialize();
            }
            else
            {
                Debug.LogWarning("InputReader is not assigned to InputReaderInitializer!");
            }
        }
    }
}
