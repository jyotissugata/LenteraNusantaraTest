using System.Collections.Generic;
using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [RequireComponent(typeof(PlayerController))]
    public class CharacterStateMachine : MonoBehaviour
    {
        [Header("State Configuration")]
        [SerializeField] private CharacterStateSO _initialState;

        [Header("Fallback State (used by Action states when no explicit next state is set)")]
        [SerializeField] private CharacterStateSO _fallbackState;
        
        [Header("Global Transitions (Any State)")]
        [SerializeField] private List<CharacterStateTransitionSO> _anyStateTransitions = new List<CharacterStateTransitionSO>();

        [Header("References")]
        [SerializeField] private Animator _animator;
        
        private PlayerController _playerController;
        private CharacterStateSO _currentState;

        public Animator Animator => _animator;
        public PlayerController PlayerController => _playerController;
        
        public Vector2 MoveInput => _playerController != null ? _playerController.MoveInput : Vector2.zero;
        public bool IsSprinting => _playerController != null && _playerController.IsSprinting;
        public bool IsGrounded => _playerController != null && _playerController.IsGrounded;

        /// <summary>
        /// The state to return to when an ActionState finishes with no explicit NextState.
        /// Typically State_Idle. The ActionState can override this per-SO.
        /// </summary>
        public CharacterStateSO FallbackState => _fallbackState;

        /// <summary>
        /// Returns true when the animation on the given layer has played to (or past) its end.
        /// Used by ActionStateSO to know when to auto-transition out.
        /// </summary>
        public bool IsActionAnimationFinished(int layerIndex = 0)
        {
            if (_animator == null) return false;
            if (_animator.IsInTransition(layerIndex)) return false;
            var info = _animator.GetCurrentAnimatorStateInfo(layerIndex);
            return info.normalizedTime >= 1f;
        }

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        private void Start()
        {
            if (_initialState != null)
            {
                TransitionTo(_initialState);
            }
        }

        private void Update()
        {
            CharacterStateSO nextState = null;

            if (_currentState == null)
            {
                TransitionTo(FallbackState);
                return;
            }
            
            if (_currentState is LoopStateSO)
            {
                // 1. Check global 'Any State' transitions first
                if (_anyStateTransitions != null)
                {
                    foreach (var transition in _anyStateTransitions)
                    {
                        // Ensure we don't infinitely re-enter the same state if already in it
                        if (transition.IsMet(this))
                        {
                            nextState = (transition.TargetState != _currentState) ? transition.TargetState : null;
                            break;
                        }
                    }
                }

                // 2. If no global transition was met, check local state transitions
                if (nextState == null)
                {
                    nextState = _currentState.CheckTransitions(this);
                }

                // 3. Transition if found
                if (nextState != null)
                {
                    TransitionTo(nextState);
                }
            }

            _currentState.OnUpdate(this);
        }

        public void TransitionTo(CharacterStateSO nextState)
        {
            if (_currentState != null)
            {
                _currentState.OnExit(this);
            }

            _currentState = nextState;

            if (_currentState != null)
            {
                _currentState.OnEnter(this);
            }
        }
    }
}