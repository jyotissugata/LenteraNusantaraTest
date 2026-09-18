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

using System.Collections;
using UnityEngine;

namespace JyotisSugata.Puzzles.NumpadPasscode
{
    public class NumpadController : MonoBehaviour, IPuzzle
    {
        [SerializeField] private NumpadView _view;

        private NumpadModel _model;

        private void Awake()
        {
            _model = new NumpadModel();
            
            _model.OnDisplayChanged += HandleDisplayChanged;
            _model.OnCodeCorrect += HandleCodeCorrect;
            _model.OnCodeIncorrect += HandleCodeIncorrect;
            
            if (_view != null)
            {
                _view.OnDigitPressed += HandleDigitPressed;
                _view.OnDeletePressed += HandleDeletePressed;
            }
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.OnDisplayChanged -= HandleDisplayChanged;
                _model.OnCodeCorrect -= HandleCodeCorrect;
                _model.OnCodeIncorrect -= HandleCodeIncorrect;
            }
            
            if (_view != null)
            {
                _view.OnDigitPressed -= HandleDigitPressed;
                _view.OnDeletePressed -= HandleDeletePressed;
            }
        }

        public void Initialize(PuzzleDefinition definition)
        {
            NumpadDefinition def = definition as NumpadDefinition;
            if (def != null && _model != null)
            {
                _model.Initialize(def.DigitCount);
            }
            
            if (_view != null)
            {
                _view.Initialize(definition);
            }
        }

        public void Cancel()
        {
            // Implementation handled by base class PuzzleUIBase via Esc key
        }

        private void HandleDisplayChanged(string display)
        {
            if (_view != null) _view.UpdateDisplay(display);
        }

        private void HandleDigitPressed(int digit)
        {
            if (_model != null) _model.InputDigit(digit);
        }

        private void HandleDeletePressed()
        {
            if (_model != null) _model.DeleteLastDigit();
        }

        private void HandleCodeCorrect()
        {
            if (_view != null) _view.SetLocked(true);
            StartCoroutine(DelayedComplete());
        }

        private IEnumerator DelayedComplete()
        {
            yield return new WaitForSecondsRealtime(0.6f);
            if (_view != null) _view.TriggerComplete();
        }

        private void HandleCodeIncorrect()
        {
            if (_view != null) _view.PlayErrorFeedback();
        }
    }
}
