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
    public abstract class PuzzleUIBase : MonoBehaviour, IPuzzle
    {
        [SerializeField] private VoidEventChannel _onPuzzleCompleted;
        [SerializeField] private VoidEventChannel _onPuzzleCanceled;
        [SerializeField] private InputReader _inputReader;

        protected virtual void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnCancelPressed += HandleCancelPressed;
            }
        }

        protected virtual void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnCancelPressed -= HandleCancelPressed;
            }
        }

        public abstract void Initialize(PuzzleDefinition definition);
        
        public virtual void Cancel()
        {
            // Implementation required by IPuzzle, handled by Esc key fundamentally via UI Base
            CancelThisPuzzle();
        }

        protected void CompleteThisPuzzle()
        {
            if (_onPuzzleCompleted != null)
            {
                _onPuzzleCompleted.Raise();
            }
        }

        protected void CancelThisPuzzle()
        {
            if (_onPuzzleCanceled != null)
            {
                _onPuzzleCanceled.Raise();
            }
        }

        private void HandleCancelPressed()
        {
            CancelThisPuzzle();
        }
    }
}
