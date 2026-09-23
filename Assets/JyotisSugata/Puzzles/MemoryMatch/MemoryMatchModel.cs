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

        // While true, FlipCard ignores all input (e.g., during flip-back animation)
        private bool _isInputLocked = false;

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
            _isInputLocked = false;
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
            if (_isInputLocked) return false;
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

        /// <summary>
        /// Called by the Controller to block/unblock input during the flip-back animation.
        /// </summary>
        public void SetInputLocked(bool locked)
        {
            _isInputLocked = locked;

            // When unlocking, also clear the selection state so fresh clicks work correctly.
            if (!locked)
            {
                _firstFlippedIndex = -1;
                _secondFlippedIndex = -1;
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

                // On match we can clear immediately — no lock needed.
                _firstFlippedIndex = -1;
                _secondFlippedIndex = -1;

                if (_matchesFound >= _totalPairs)
                {
                    OnAllPairsMatched?.Invoke();
                }
            }
            else
            {
                // Lock input BEFORE notifying — the Controller's coroutine will unlock after animation.
                _isInputLocked = true;
                OnPairMismatched?.Invoke(a, b);
                // NOTE: _firstFlippedIndex/_secondFlippedIndex are intentionally NOT reset here.
                // SetInputLocked(false) will reset them once the animation finishes.
            }
        }

        public List<CardData> GetCards()
        {
            return _cards;
        }

        public void ResetMismatchedPair(int a, int b)
        {
            if (a >= 0 && a < _cards.Count) _cards[a].IsFaceUp = false;
            if (b >= 0 && b < _cards.Count) _cards[b].IsFaceUp = false;
        }
    }
}
