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

namespace JyotisSugata.Puzzles.Shared
{
    [CreateAssetMenu(menuName = "JyotisSugata/Puzzles/Puzzle Definition")]
    public class PuzzleDefinition : ScriptableObject
    {
        [SerializeField] private PuzzleType _puzzleType;
        [SerializeField] private GameObject _puzzleUIPrefab;

        public PuzzleType PuzzleType => _puzzleType;
        public GameObject PuzzleUIPrefab => _puzzleUIPrefab;
    }
}
