using System;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace JyotisSugata.Puzzles.MemoryMatch
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _frontImage;
        [SerializeField] private Image _backImage;
        [SerializeField] private float _flipDuration = 0.3f;

        private int _cardIndex;
        private bool _isAnimating;

        public event Action<int> OnCardClicked;

        public void Setup(int cardIndex, Sprite frontSprite)
        {
            _cardIndex = cardIndex;
            if (_frontImage != null) _frontImage.sprite = frontSprite;
            ShowBack();
            
            if (_button != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => OnCardClicked?.Invoke(_cardIndex));
            }
        }

        /// <summary>
        /// Updates the card's icon sprite. Used when icons are assigned
        /// after creation based on the shuffled model data.
        /// </summary>
        public void SetIcon(Sprite sprite)
        {
            if (_frontImage != null) _frontImage.sprite = sprite;
        }

        public void FlipToFront()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            
            transform.DOScaleX(0, _flipDuration / 2f).SetUpdate(true).OnComplete(() =>
            {
                ShowFront();
                transform.DOScaleX(1, _flipDuration / 2f).SetUpdate(true).OnComplete(() => _isAnimating = false);
            });
        }

        public void FlipToBack()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            
            transform.DOScaleX(0, _flipDuration / 2f).SetUpdate(true).OnComplete(() =>
            {
                ShowBack();
                transform.DOScaleX(1, _flipDuration / 2f).SetUpdate(true).OnComplete(() => _isAnimating = false);
            });
        }

        public float FlipDuration => _flipDuration;

        public void SetMatched()
        {
            if (_frontImage != null)
            {
                _frontImage.DOColor(new Color(0.5f, 1f, 0.5f, 1f), 0.3f).SetUpdate(true);
            }
            if (_button != null)
            {
                _button.interactable = false;
            }
        }

        private void ShowBack()
        {
            if (_frontImage != null) _frontImage.gameObject.SetActive(false);
            if (_backImage != null) _backImage.gameObject.SetActive(true);
        }

        private void ShowFront()
        {
            if (_frontImage != null) _frontImage.gameObject.SetActive(true);
            if (_backImage != null) _backImage.gameObject.SetActive(false);
        }
    }
}
