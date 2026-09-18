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
using System.Collections.Generic;
using UnityEngine;

namespace JyotisSugata.Puzzles.MemoryMatch
{
    public class MemoryMatchView : PuzzleUIBase
    {
        [SerializeField] private Transform _cardGridParent;
        [SerializeField] private CardView _cardViewPrefab;

        private List<CardView> _cardViews = new List<CardView>();

        public event Action<int> OnCardClickedInView;

        public override void Initialize(PuzzleDefinition definition)
        {
            MemoryMatchDefinition def = definition as MemoryMatchDefinition;
            if (def == null) return;

            foreach (var view in _cardViews)
            {
                if (view != null) Destroy(view.gameObject);
            }
            _cardViews.Clear();

            int totalCards = def.Columns * def.Rows;
            int iconCount = def.CardIcons.Count;

            for (int i = 0; i < totalCards; i++)
            {
                CardView cardView = Instantiate(_cardViewPrefab, _cardGridParent);
                Sprite icon = iconCount > 0 ? def.CardIcons[i % iconCount] : null;
                cardView.Setup(i, icon);
                cardView.OnCardClicked += HandleCardClicked;
                _cardViews.Add(cardView);
            }
        }

        public void ShowCard(int index, bool faceUp)
        {
            if (index < 0 || index >= _cardViews.Count) return;

            if (faceUp)
            {
                _cardViews[index].FlipToFront();
            }
            else
            {
                _cardViews[index].FlipToBack();
            }
        }

        public void MarkCardMatched(int index)
        {
            if (index < 0 || index >= _cardViews.Count) return;
            _cardViews[index].SetMatched();
        }

        public void TriggerComplete()
        {
            CompleteThisPuzzle();
        }

        private void HandleCardClicked(int index)
        {
            OnCardClickedInView?.Invoke(index);
        }
    }
}
