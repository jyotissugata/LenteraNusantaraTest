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

using UnityEngine;

namespace JyotisSugata.Core.StateMachine
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PuzzleRequestEventChannel _onPuzzleRequested;
        [SerializeField] private VoidEventChannel _onPuzzleCompleted;
        [SerializeField] private VoidEventChannel _onPuzzleCanceled;
        [SerializeField] private GameStateEventChannel _onGameStateChanged;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private ScreenTransitionController _screenTransitionController;

        private IGameState _currentState;
        private ExplorationState _explorationState;
        private PuzzleState _puzzleState;
        
        private static GameStateManager _instance;

        public static GameStateManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            
            _explorationState = new ExplorationState(_inputReader);
            _puzzleState = new PuzzleState(_inputReader, _screenTransitionController);
            
            if (_onPuzzleRequested != null)
                _onPuzzleRequested.Subscribe(HandlePuzzleRequested);
                
            if (_onPuzzleCompleted != null)
                _onPuzzleCompleted.Subscribe(HandlePuzzleCompletedOrCanceled);
                
            if (_onPuzzleCanceled != null)
                _onPuzzleCanceled.Subscribe(HandlePuzzleCompletedOrCanceled);
        }

        private void Start()
        {
            TransitionToState(_explorationState, GameStateType.Exploration);
        }

        private void OnDestroy()
        {
            if (_onPuzzleRequested != null)
                _onPuzzleRequested.Unsubscribe(HandlePuzzleRequested);
                
            if (_onPuzzleCompleted != null)
                _onPuzzleCompleted.Unsubscribe(HandlePuzzleCompletedOrCanceled);
                
            if (_onPuzzleCanceled != null)
                _onPuzzleCanceled.Unsubscribe(HandlePuzzleCompletedOrCanceled);
        }

        private void HandlePuzzleRequested(PuzzleDefinition definition)
        {
            _puzzleState.SetPuzzleDefinition(definition);
            TransitionToState(_puzzleState, GameStateType.Puzzle);
        }

        private void HandlePuzzleCompletedOrCanceled()
        {
            TransitionToState(_explorationState, GameStateType.Exploration);
        }

        private void TransitionToState(IGameState newState, GameStateType stateType)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
            
            if (_onGameStateChanged != null)
            {
                _onGameStateChanged.Raise(stateType);
            }
        }
    }
}
