using UnityEngine;
using JyotisSugata.Core.Events;
using JyotisSugata.Core.Input;
using JyotisSugata.Exploration.Player.StateMachine;

namespace JyotisSugata.Exploration.Interaction
{
    public class InteractionDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private BoolEventChannel _onInteractionPromptChanged;

        [Header("Interaction Animation (Optional)")]
        [Tooltip("Assign an ActionStateSO to play an animation when player interacts.")]
        [SerializeField] private ActionStateSO _interactAnimationState;
        
        [Header("Settings")]
        [SerializeField] private float _detectionRadius = 2.5f;
        [SerializeField] private LayerMask _interactableLayer;

        private IInteractable _currentInteractable;
        private Collider[] _overlapResults = new Collider[5];
        private CharacterStateMachine _characterStateMachine;

        private void Awake()
        {
            // CharacterStateMachine lives on the same [Player] GameObject
            _characterStateMachine = GetComponent<CharacterStateMachine>();
        }

        private void OnEnable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnInteractPressed += HandleInteractPressed;
            }
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.OnInteractPressed -= HandleInteractPressed;
            }
        }

        private void Update()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _overlapResults, _interactableLayer);
            
            IInteractable closestInteractable = null;
            float closestDistanceSqr = Mathf.Infinity;

            for (int i = 0; i < count; i++)
            {
                IInteractable interactable = _overlapResults[i].GetComponent<IInteractable>();
                if (interactable != null)
                {
                    float distanceSqr = (transform.position - _overlapResults[i].transform.position).sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestInteractable = interactable;
                    }
                }
            }

            if (closestInteractable != _currentInteractable)
            {
                if (_currentInteractable != null)
                {
                    var oldObj = _currentInteractable as InteractableObject;
                    if (oldObj != null) oldObj.SetPlayerInRange(false);
                }

                _currentInteractable = closestInteractable;

                if (_currentInteractable != null)
                {
                    var newObj = _currentInteractable as InteractableObject;
                    if (newObj != null) newObj.SetPlayerInRange(true);
                }

                if (_onInteractionPromptChanged != null)
                {
                    _onInteractionPromptChanged.Raise(_currentInteractable != null);
                }
            }
        }

        private void HandleInteractPressed()
        {
            if (_currentInteractable != null)
            {
                // Play interact animation first (if assigned), then trigger the puzzle
                if (_interactAnimationState != null && _characterStateMachine != null)
                {
                    _characterStateMachine.TransitionTo(_interactAnimationState);
                }

                _currentInteractable.Interact();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}
