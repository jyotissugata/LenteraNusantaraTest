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

namespace JyotisSugata.Exploration.Interaction
{
    public interface IInteractable
    {
        void Interact();
        string GetPromptText();
    }
}
