using System.Collections;

using UnityEngine;

using JyotisSugata.Puzzles.Shared;

namespace JyotisSugata.UI.Transitions
{
    public class ScreenTransitionController : MonoBehaviour
    {
        [SerializeField] private PuzzlePanelAnimator _panelAnimator;
        [SerializeField] private Canvas _puzzleRootCanvas;
        [SerializeField] private Transform _puzzleUIPanelParent;

        private GameObject _currentPuzzleInstance;
        private Coroutine _transitionCoroutine;
        private CanvasGroup _puzzleRootCanvasGroup;

        private void Awake()
        {
            // Use CanvasGroup to hide/show the canvas instead of SetActive,
            // so this MonoBehaviour (which may live on the same GO) stays alive.
            if (_puzzleRootCanvas != null)
            {
                _puzzleRootCanvasGroup = _puzzleRootCanvas.GetComponent<CanvasGroup>();
                if (_puzzleRootCanvasGroup == null)
                    _puzzleRootCanvasGroup = _puzzleRootCanvas.gameObject.AddComponent<CanvasGroup>();

                // Start hidden
                SetCanvasVisible(false);
            }
        }

        public void TransitionToPuzzle(PuzzleDefinition definition)
        {
            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = StartCoroutine(TransitionInCoroutine(definition));
        }

        public void TransitionToExploration()
        {
            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = StartCoroutine(TransitionOutCoroutine());
        }

        private IEnumerator TransitionInCoroutine(PuzzleDefinition definition)
        {
            _panelAnimator.FadeIn();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            
            // Clean up old instance if we interrupted a transition out
            if (_currentPuzzleInstance != null)
            {
                Destroy(_currentPuzzleInstance);
                _currentPuzzleInstance = null;
            }

            SetCanvasVisible(true);
            _currentPuzzleInstance = Instantiate(definition.PuzzleUIPrefab, _puzzleUIPanelParent);
            
            IPuzzle puzzle = _currentPuzzleInstance.GetComponent<IPuzzle>();
            puzzle?.Initialize(definition);
            
            _panelAnimator.FadeOut();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
        }

        private IEnumerator TransitionOutCoroutine()
        {
            _panelAnimator.FadeIn();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
            
            if (_currentPuzzleInstance != null)
            {
                Destroy(_currentPuzzleInstance);
                _currentPuzzleInstance = null;
            }
            
            SetCanvasVisible(false);
            
            _panelAnimator.FadeOut();
            
            yield return new WaitForSecondsRealtime(_panelAnimator.FadeDuration);
        }

        private void SetCanvasVisible(bool visible)
        {
            if (_puzzleRootCanvasGroup == null) return;
            _puzzleRootCanvasGroup.alpha = visible ? 1f : 0f;
            _puzzleRootCanvasGroup.interactable = visible;
            _puzzleRootCanvasGroup.blocksRaycasts = visible;
        }
    }
}
