using System;

using UnityEngine;

namespace JyotisSugata.Core.Input
{
    [CreateAssetMenu(menuName = "JyotisSugata/Input/Input Reader")]
    public class InputReader : ScriptableObject
    {
        private PlayerInputActions _inputActions;

        public event Action<Vector2> OnMoveInput;
        public event Action<Vector2> OnLookInput;
        public event Action OnInteractPressed;
        public event Action OnCancelPressed;
        public event Action<bool> OnSprintChanged;

        public void Initialize()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerInputActions();
                
                _inputActions.Player.Move.performed += ctx => OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
                _inputActions.Player.Move.canceled += ctx => OnMoveInput?.Invoke(Vector2.zero);
                
                _inputActions.Player.Look.performed += ctx => OnLookInput?.Invoke(ctx.ReadValue<Vector2>());
                _inputActions.Player.Look.canceled += ctx => OnLookInput?.Invoke(Vector2.zero);
                
                _inputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
                
                _inputActions.Player.Sprint.performed += ctx => OnSprintChanged?.Invoke(true);
                _inputActions.Player.Sprint.canceled += ctx => OnSprintChanged?.Invoke(false);
                
                _inputActions.UI.Cancel.performed += ctx => OnCancelPressed?.Invoke();
            }
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void EnablePlayerInput()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Enable();
        }

        public void EnableUIInput()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Enable();
        }

        public void DisableAllInput()
        {
            if (_inputActions != null)
            {
                _inputActions.Player.Disable();
                _inputActions.UI.Disable();
            }
        }
    }
}
