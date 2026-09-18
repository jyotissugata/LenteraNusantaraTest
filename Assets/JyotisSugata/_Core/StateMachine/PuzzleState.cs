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
    public class PuzzleState : IGameState
    {
        private InputReader _inputReader;
        private ScreenTransitionController _transitionController;
        private PuzzleDefinition _pendingDefinition;

        public PuzzleState(InputReader inputReader, ScreenTransitionController transitionController)
        {
            _inputReader = inputReader;
            _transitionController = transitionController;
        }

        public void SetPuzzleDefinition(PuzzleDefinition definition)
        {
            _pendingDefinition = definition;
        }

        public void Enter()
        {
            if (_inputReader != null)
            {
                _inputReader.EnableUIInput();
            }
            
            if (_transitionController != null && _pendingDefinition != null)
            {
                _transitionController.TransitionToPuzzle(_pendingDefinition);
            }
        }

        public void Exit()
        {
            if (_transitionController != null)
            {
                _transitionController.TransitionToExploration();
            }
        }
    }
}
