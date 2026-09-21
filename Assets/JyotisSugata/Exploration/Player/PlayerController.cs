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
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _rotationSmoothTime = 0.12f;
        [SerializeField] private float _gravity = -15f;
        
        [Header("References")]
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private Transform _mainCamera;

        private CharacterController _characterController;
        private Vector2 _moveInput;
        private bool _isSprinting;
        private float _currentMoveSpeed = 4f;
        private float _verticalVelocity;
        private float _rotationVelocity;
        private bool _isActive = true;

        public Vector2 MoveInput => _moveInput;
        public bool IsSprinting => _isSprinting;
        public bool IsGrounded => _characterController != null && _characterController.isGrounded;

        public void SetMoveSpeed(float speed)
        {
            _currentMoveSpeed = speed;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            
            // Auto-assign Main Camera if not set
            if (_mainCamera == null && Camera.main != null)
            {
                _mainCamera = Camera.main.transform;
            }
        }

        private void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnMoveInput += HandleMoveInput;
                _inputReader.OnSprintChanged += HandleSprintChanged;
            }
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnMoveInput -= HandleMoveInput;
                _inputReader.OnSprintChanged -= HandleSprintChanged;
            }
        }

        private void Update()
        {
            if (!_isActive) return;

            // Handle Gravity
            if (_characterController.isGrounded)
            {
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }

            // Handle Movement (Camera-Relative)
            float speed = _currentMoveSpeed;
            Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

            if (inputDirection.magnitude >= 0.1f)
            {
                // 1. Calculate target rotation based on input and camera's yaw
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                
                if (_mainCamera != null)
                {
                    targetAngle += _mainCamera.eulerAngles.y;
                }

                // 2. Smoothly rotate character model to face movement direction
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

                // 3. Move character
                Vector3 moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                _characterController.Move(moveDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
            else
            {
                // Apply gravity only if not moving
                _characterController.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
        }

        private void HandleMoveInput(Vector2 input)
        {
            _moveInput = input;
        }

        private void HandleSprintChanged(bool sprinting)
        {
            _isSprinting = sprinting;
        }

        public void SetActive(bool active)
        {
            _isActive = active;
        }
    }
}
