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
using DG.Tweening;

namespace JyotisSugata.UI.Transitions
{
    public class PuzzlePanelAnimator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _fadeOverlay;
        [SerializeField] private float _fadeDuration = 0.4f;

        public float FadeDuration => _fadeDuration;

        private void Awake()
        {
            if (_fadeOverlay != null)
            {
                _fadeOverlay.alpha = 0f;
                _fadeOverlay.gameObject.SetActive(false);
            }
        }

        public void FadeIn()
        {
            if (_fadeOverlay != null)
            {
                _fadeOverlay.DOKill();
                _fadeOverlay.gameObject.SetActive(true);
                _fadeOverlay.DOFade(1f, _fadeDuration).SetUpdate(true);
            }
        }

        public void FadeOut()
        {
            if (_fadeOverlay != null)
            {
                _fadeOverlay.DOKill();
                _fadeOverlay.DOFade(0f, _fadeDuration).SetUpdate(true)
                    .OnComplete(() => _fadeOverlay.gameObject.SetActive(false));
            }
        }
    }
}
