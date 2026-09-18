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

namespace JyotisSugata.UI.Transitions
{
    public class ScreenTransitionController : MonoBehaviour
    {
        [SerializeField] private PuzzlePanelAnimator _panelAnimator;
        [SerializeField] private Canvas _puzzleRootCanvas;
        [SerializeField] private Transform _puzzleUIPanelParent;

        private GameObject _currentPuzzleInstance;
        private bool _isTransitioning = false;

        public void TransitionToPuzzle(PuzzleDefinition definition)
        {
            if (_isTransitioning) return;
            StartCoroutine(TransitionInCoroutine(definition));
        }

        public void TransitionToExploration()
        {
            if (_isTransitioning) return;
            StartCoroutine(TransitionOutCoroutine());
        }

        private IEnumerator TransitionInCoroutine(PuzzleDefinition definition)
        {
            _isTransitioning = true;
            _panelAnimator.FadeIn();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            
            _puzzleRootCanvas.gameObject.SetActive(true);
            _currentPuzzleInstance = Instantiate(definition.PuzzleUIPrefab, _puzzleUIPanelParent);
            
            IPuzzle puzzle = _currentPuzzleInstance.GetComponent<IPuzzle>();
            puzzle?.Initialize(definition);
            
            _panelAnimator.FadeOut();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            _isTransitioning = false;
        }

        private IEnumerator TransitionOutCoroutine()
        {
            _isTransitioning = true;
            _panelAnimator.FadeIn();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            
            if (_currentPuzzleInstance != null)
            {
                Destroy(_currentPuzzleInstance);
                _currentPuzzleInstance = null;
            }
            
            _puzzleRootCanvas.gameObject.SetActive(false);
            
            _panelAnimator.FadeOut();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            _isTransitioning = false;
        }
    }
}
