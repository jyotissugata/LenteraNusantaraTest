using UnityEngine;

using JyotisSugata.Puzzles.Shared;

namespace JyotisSugata.Puzzles.NumpadPasscode
{
    [CreateAssetMenu(menuName = "JyotisSugata/Puzzles/Numpad Passcode Definition")]
    public class NumpadDefinition : PuzzleDefinition
    {
        [SerializeField] private int _digitCount = 4;

        public int DigitCount => _digitCount;
    }
}
