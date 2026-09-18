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

namespace JyotisSugata.Puzzles.NumpadPasscode
{
    [CreateAssetMenu(menuName = "JyotisSugata/Puzzles/Numpad Passcode Definition")]
    public class NumpadDefinition : PuzzleDefinition
    {
        [SerializeField] private int _digitCount = 4;

        public int DigitCount => _digitCount;
    }
}
