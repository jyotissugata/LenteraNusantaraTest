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

namespace JyotisSugata.Core.StateMachine
{
    public class ExplorationState : IGameState
    {
        private InputReader _inputReader;

        public ExplorationState(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public void Enter()
        {
            if (_inputReader != null)
            {
                _inputReader.EnablePlayerInput();
            }
        }

        public void Exit()
        {
            // InputReader will be switched by PuzzleState
        }
    }
}
