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
using UnityEngine.UI;

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

            // Update GridLayoutGroup column count dynamically from definition
            GridLayoutGroup gridLayout = _cardGridParent.GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = def.Columns;
            }

            // Destroy any existing cards
            foreach (var view in _cardViews)
            {
                if (view != null) Destroy(view.gameObject);
            }
            _cardViews.Clear();

            // Create card views without icons — icons will be assigned after
            // the model shuffles, via SetupCardIcons()
            int totalCards = def.Columns * def.Rows;
            for (int i = 0; i < totalCards; i++)
            {
                CardView cardView = Instantiate(_cardViewPrefab, _cardGridParent);
                cardView.Setup(i, null);
                cardView.OnCardClicked += HandleCardClicked;
                _cardViews.Add(cardView);
            }
        }

        /// <summary>
        /// Called by the Controller after the Model has shuffled the cards.
        /// Assigns the correct icon to each card based on its PairId,
        /// so visually identical icons will always match in the game logic.
        /// </summary>
        public void SetupCardIcons(List<MemoryMatchModel.CardData> shuffledCards, List<Sprite> icons)
        {
            int iconCount = icons != null ? icons.Count : 0;
            for (int i = 0; i < _cardViews.Count && i < shuffledCards.Count; i++)
            {
                Sprite icon = iconCount > 0 ? icons[shuffledCards[i].PairId % iconCount] : null;
                _cardViews[i].SetIcon(icon);
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
