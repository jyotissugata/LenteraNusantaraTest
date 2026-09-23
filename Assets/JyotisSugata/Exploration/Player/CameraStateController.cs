using UnityEngine;

using Unity.Cinemachine;

using JyotisSugata.Core.Events;
using JyotisSugata.Core.Input;
using JyotisSugata.Core.StateMachine;
using JyotisSugata.Exploration.Interaction;

namespace JyotisSugata.Exploration.Player
{
    public class CameraStateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameStateEventChannel _onGameStateChanged;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private CinemachineCamera _explorationCamera; 
        [SerializeField] private CinemachineCamera _puzzleCamera;
        [SerializeField] private Transform _cameraTarget;

        [Header("Settings")]
        [SerializeField] private float _lookSensitivityX = 1f;
        [SerializeField] private float _lookSensitivityY = 1f;
        [SerializeField] private float _pitchMin = -30f;
        [SerializeField] private float _pitchMax = 70f;

        private Vector2 _lookInput;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private bool _isActive = false;
        private Transform _lastInteractableTarget;

        private void Start()
        {
            if (_cameraTarget != null)
            {
                _cinemachineTargetYaw = _cameraTarget.eulerAngles.y;
            }
        }

        private void OnEnable()
        {
            if (_onGameStateChanged != null)
            {
                _onGameStateChanged.Subscribe(HandleStateChanged);
            }
            if (_inputReader != null)
            {
                _inputReader.OnLookInput += HandleLookInput;
            }
            InteractableObject.OnInteractionStarted += HandleInteractionStarted;
        }

        private void OnDisable()
        {
            if (_onGameStateChanged != null)
            {
                _onGameStateChanged.Unsubscribe(HandleStateChanged);
            }
            if (_inputReader != null)
            {
                _inputReader.OnLookInput -= HandleLookInput;
            }
            InteractableObject.OnInteractionStarted -= HandleInteractionStarted;
        }

        private void HandleInteractionStarted(Transform target)
        {
            _lastInteractableTarget = target;
        }

        private void LateUpdate()
        {
            if (!_isActive || _cameraTarget == null) return;

            if (_lookInput.sqrMagnitude >= 0.01f)
            {
                // Multiply by Time.deltaTime to make it framerate independent.
                // Multiply by 50f so the default Inspector value of "1" feels natural.
                const float internalMultiplier = 50f;
                _cinemachineTargetYaw += _lookInput.x * _lookSensitivityX * internalMultiplier * Time.deltaTime;
                _cinemachineTargetPitch -= _lookInput.y * _lookSensitivityY * internalMultiplier * Time.deltaTime;
            }

            _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, _pitchMin, _pitchMax);

            if (_cinemachineTargetYaw < -360f) _cinemachineTargetYaw += 360f;
            if (_cinemachineTargetYaw > 360f) _cinemachineTargetYaw -= 360f;

            _cameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0f);
        }

        private void HandleStateChanged(GameStateType state)
        {
            if (state == GameStateType.Exploration)
            {
                _isActive = true;
                if (_explorationCamera != null) _explorationCamera.Priority = 11;
                if (_puzzleCamera != null) _puzzleCamera.Priority = 9;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (state == GameStateType.Puzzle)
            {
                _isActive = false;
                if (_explorationCamera != null) _explorationCamera.Priority = 9;
                
                // Reposition the puzzle camera dynamically to frame the object
                if (_puzzleCamera != null && _lastInteractableTarget != null)
                {
                    // Place camera 1.5 units in front and slightly above the object
                    Vector3 idealPos = _lastInteractableTarget.position + _lastInteractableTarget.forward * 1.5f + Vector3.up * 1.0f;
                    _puzzleCamera.transform.position = idealPos;
                    _puzzleCamera.transform.LookAt(_lastInteractableTarget);
                    
                    _puzzleCamera.Priority = 11;
                }
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void HandleLookInput(Vector2 input)
        {
            _lookInput = input;
        }
    }
}
