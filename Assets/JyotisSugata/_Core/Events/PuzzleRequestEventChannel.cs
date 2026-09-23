using UnityEngine;

using JyotisSugata.Puzzles.Shared;

namespace JyotisSugata.Core.Events
{
    [CreateAssetMenu(menuName = "JyotisSugata/Events/Puzzle Request Event Channel")]
    public class PuzzleRequestEventChannel : EventChannelBase<PuzzleDefinition>
    {
    }
}
