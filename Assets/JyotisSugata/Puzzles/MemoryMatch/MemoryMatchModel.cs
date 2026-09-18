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

namespace JyotisSugata.Puzzles.MemoryMatch
{
    public class MemoryMatchModel
    {
        public class CardData
        {
            public int Index;
            public int PairId;
            public bool IsFaceUp;
            public bool IsMatched;
        }

        private List<CardData> _cards = new List<CardData>();
        private int _firstFlippedIndex = -1;
        private int _secondFlippedIndex = -1;
        private int _matchesFound = 0;
        private int _totalPairs;

        public event Action<int, bool> OnCardFlipped;
        public event Action<int, int> OnPairMatched;
        public event Action<int, int> OnPairMismatched;
        public event Action OnAllPairsMatched;

        public void Initialize(int columns, int rows, int pairCount)
        {
            _totalPairs = pairCount;
            _matchesFound = 0;
            _firstFlippedIndex = -1;
            _secondFlippedIndex = -1;
            _cards.Clear();

            List<CardData> tempCards = new List<CardData>();
            for (int i = 0; i < pairCount; i++)
            {
                tempCards.Add(new CardData { PairId = i, IsFaceUp = false, IsMatched = false });
                tempCards.Add(new CardData { PairId = i, IsFaceUp = false, IsMatched = false });
            }

            Random rand = new Random(Environment.TickCount);
            int n = tempCards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                CardData value = tempCards[k];
                tempCards[k] = tempCards[n];
                tempCards[n] = value;
            }

            for (int i = 0; i < tempCards.Count; i++)
            {
                tempCards[i].Index = i;
                _cards.Add(tempCards[i]);
            }
        }

        public bool CanFlipCard(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex >= _cards.Count) return false;
            return !_cards[cardIndex].IsFaceUp && !_cards[cardIndex].IsMatched && _secondFlippedIndex == -1;
        }

        public void FlipCard(int cardIndex)
        {
            if (!CanFlipCard(cardIndex)) return;

            _cards[cardIndex].IsFaceUp = true;
            OnCardFlipped?.Invoke(cardIndex, true);

            if (_firstFlippedIndex == -1)
            {
                _firstFlippedIndex = cardIndex;
            }
            else
            {
                _secondFlippedIndex = cardIndex;
                EvaluatePair();
            }
        }

        private void EvaluatePair()
        {
            int a = _firstFlippedIndex;
            int b = _secondFlippedIndex;

            if (_cards[a].PairId == _cards[b].PairId)
            {
                _cards[a].IsMatched = true;
                _cards[b].IsMatched = true;
                _matchesFound++;
                OnPairMatched?.Invoke(a, b);
                
                if (_matchesFound >= _totalPairs)
                {
                    OnAllPairsMatched?.Invoke();
                }
            }
            else
            {
                OnPairMismatched?.Invoke(a, b);
            }

            _firstFlippedIndex = -1;
            _secondFlippedIndex = -1;
        }

        public List<CardData> GetCards()
        {
            return _cards;
        }
    }
}
