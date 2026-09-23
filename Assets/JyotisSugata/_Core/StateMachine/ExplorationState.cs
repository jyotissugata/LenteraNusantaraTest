using JyotisSugata.Core.Input;

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
