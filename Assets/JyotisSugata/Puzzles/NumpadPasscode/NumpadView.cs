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

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace JyotisSugata.Puzzles.NumpadPasscode
{
    public class NumpadView : PuzzleUIBase
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private Button[] _digitButtons = new Button[10];
        [SerializeField] private Button _deleteButton;
        [SerializeField] private CanvasGroup _panelCanvasGroup;

        private bool _isLocked = false;

        public event Action<int> OnDigitPressed;
        public event Action OnDeletePressed;

        public override void Initialize(PuzzleDefinition definition)
        {
            for (int i = 0; i < 10; i++)
            {
                int d = i;
                if (i < _digitButtons.Length && _digitButtons[i] != null)
                {
                    _digitButtons[i].onClick.RemoveAllListeners();
                    _digitButtons[i].onClick.AddListener(() =>
                    {
                        if (!_isLocked) OnDigitPressed?.Invoke(d);
                    });
                }
            }

            if (_deleteButton != null)
            {
                _deleteButton.onClick.RemoveAllListeners();
                _deleteButton.onClick.AddListener(() =>
                {
                    if (!_isLocked) OnDeletePressed?.Invoke();
                });
            }
        }

        public void UpdateDisplay(string text)
        {
            if (_displayText != null)
            {
                _displayText.text = text;
            }
        }

        public void UpdateHint(string secretCode)
        {
            if (_hintText != null)
            {
                _hintText.text = $"Hint: {secretCode}";
            }
        }

        public void PlayErrorFeedback()
        {
            if (_panelCanvasGroup != null)
            {
                _panelCanvasGroup.transform.DOShakePosition(0.4f, strength: 10f, vibrato: 20, snapping: false).SetUpdate(true);
            }
            
            if (_displayText != null)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(_displayText.DOColor(Color.red, 0.1f));
                seq.Append(_displayText.DOColor(Color.white, 0.2f));
                seq.SetUpdate(true);
            }
        }

        public void SetLocked(bool locked)
        {
            _isLocked = locked;
        }

        public void TriggerComplete()
        {
            CompleteThisPuzzle();
        }
    }
}
