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

namespace JyotisSugata.Exploration.Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private InputReader _inputReader;

        private Animator _animator;
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private Vector2 _currentMoveInput;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
            {
                Debug.LogWarning("PlayerAnimatorController: No Animator found on Player or its children!");
            }
        }

        private void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnMoveInput += HandleMoveInput;
            }
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnMoveInput -= HandleMoveInput;
            }
        }

        private void Update()
        {
            _animator.SetFloat(SpeedHash, _playerController.IsSprinting ? _currentMoveInput.magnitude : _currentMoveInput.magnitude / 2f, 0.1f, Time.deltaTime);
        }

        private void HandleMoveInput(Vector2 input)
        {
            _currentMoveInput = input;
        }
    }
}
