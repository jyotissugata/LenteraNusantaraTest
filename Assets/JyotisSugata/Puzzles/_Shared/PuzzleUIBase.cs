using UnityEngine;

using JyotisSugata.Core.Events;
using JyotisSugata.Core.Input;

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
