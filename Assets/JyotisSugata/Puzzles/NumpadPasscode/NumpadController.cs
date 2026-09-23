using System.Collections;
using UnityEngine;
using JyotisSugata.Puzzles.Shared;

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
                if (_model != null)
                {
                    _view.UpdateHint(_model.SecretCode);
                }
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
