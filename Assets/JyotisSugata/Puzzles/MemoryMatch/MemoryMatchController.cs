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

namespace JyotisSugata.Puzzles.MemoryMatch
{
    public class MemoryMatchController : MonoBehaviour, IPuzzle
    {
        [SerializeField] private MemoryMatchView _view;

        private MemoryMatchModel _model;

        private void Awake()
        {
            _model = new MemoryMatchModel();
            
            _model.OnCardFlipped += HandleCardFlipped;
            _model.OnPairMatched += HandlePairMatched;
            _model.OnPairMismatched += HandlePairMismatched;
            _model.OnAllPairsMatched += HandleAllPairsMatched;
            
            if (_view != null)
            {
                _view.OnCardClickedInView += HandleCardClicked;
            }
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.OnCardFlipped -= HandleCardFlipped;
                _model.OnPairMatched -= HandlePairMatched;
                _model.OnPairMismatched -= HandlePairMismatched;
                _model.OnAllPairsMatched -= HandleAllPairsMatched;
            }
            
            if (_view != null)
            {
                _view.OnCardClickedInView -= HandleCardClicked;
            }
        }

        public void Setup(MemoryMatchDefinition definition)
        {
            if (definition == null) return;
            
            int pairs = (definition.Columns * definition.Rows) / 2;
            _model.Initialize(definition.Columns, definition.Rows, pairs);
            
            if (_view != null)
            {
                _view.Initialize(definition);
            }
        }

        public void Initialize(PuzzleDefinition definition)
        {
            MemoryMatchDefinition def = definition as MemoryMatchDefinition;
            if (def != null)
            {
                Setup(def);
            }
        }

        public void Cancel()
        {
            // Implementation handled by PuzzleUIBase via Esc key
        }

        private void HandleCardClicked(int index)
        {
            _model.FlipCard(index);
        }

        private void HandleCardFlipped(int index, bool isFaceUp)
        {
            _view.ShowCard(index, isFaceUp);
        }

        private void HandlePairMatched(int a, int b)
        {
            _view.MarkCardMatched(a);
            _view.MarkCardMatched(b);
        }

        private void HandlePairMismatched(int a, int b)
        {
            StartCoroutine(FlipBackAfterDelay(a, b));
        }

        private IEnumerator FlipBackAfterDelay(int a, int b)
        {
            yield return new WaitForSecondsRealtime(1f);
            if (_view != null)
            {
                _view.ShowCard(a, false);
                _view.ShowCard(b, false);
            }
        }

        private void HandleAllPairsMatched()
        {
            StartCoroutine(DelayedComplete());
        }

        private IEnumerator DelayedComplete()
        {
            yield return new WaitForSecondsRealtime(0.8f);
            if (_view != null)
            {
                _view.TriggerComplete();
            }
        }
    }
}
