using UnityEngine;

namespace JyotisSugata.Puzzles.Shared
{
    [CreateAssetMenu(menuName = "JyotisSugata/Puzzles/Puzzle Definition")]
    public class PuzzleDefinition : ScriptableObject
    {
        [SerializeField] private PuzzleType _puzzleType;
        [SerializeField] private GameObject _puzzleUIPrefab;

        public PuzzleType PuzzleType => _puzzleType;
        public GameObject PuzzleUIPrefab => _puzzleUIPrefab;
    }
}
