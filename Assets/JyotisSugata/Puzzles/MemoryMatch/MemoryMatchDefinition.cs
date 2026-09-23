using System.Collections.Generic;

using UnityEngine;

using JyotisSugata.Puzzles.Shared;

namespace JyotisSugata.Puzzles.MemoryMatch
{
    [CreateAssetMenu(menuName = "JyotisSugata/Puzzles/Memory Match Definition")]
    public class MemoryMatchDefinition : PuzzleDefinition
    {
        [SerializeField] private int _columns = 4;
        [SerializeField] private int _rows = 4;
        [SerializeField] private List<Sprite> _cardIcons = new List<Sprite>();

        public int Columns => _columns;
        public int Rows => _rows;
        public List<Sprite> CardIcons => _cardIcons;
    }
}
