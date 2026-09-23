

namespace JyotisSugata.Puzzles.Shared
{
    public interface IPuzzle
    {
        void Initialize(PuzzleDefinition definition);
        void Cancel();
    }
}
