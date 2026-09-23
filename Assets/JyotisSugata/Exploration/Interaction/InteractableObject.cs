using UnityEngine;
using UnityEngine.Events;

using JyotisSugata.Core.Events;
using JyotisSugata.Puzzles.Shared;

namespace JyotisSugata.Exploration.Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private PuzzleRequestEventChannel _onPuzzleRequested;
        [SerializeField] private PuzzleDefinition _puzzleDefinition;
        [SerializeField] private GameObject _vfxSparkPrefab;
        
        [Header("Settings")]
        [SerializeField] private string _promptText = "Press E to Interact";

        [Header("Feedback Events")]
        public UnityEvent OnPuzzleSolved;
        public UnityEvent OnPuzzleCanceled;

        private GameObject _activeVFX;
        private bool _playerInRange = false;

        public static System.Action<Transform> OnInteractionStarted;
        
        // Track the currently active interactable so the GameStateManager can tell it when it's done
        public static InteractableObject CurrentActiveInteractable { get; private set; }

        public void Interact()
        {
            if (_puzzleDefinition == null)
            {
                Debug.LogWarning($"PuzzleDefinition is missing on {gameObject.name}!");
                return;
            }
            
            CurrentActiveInteractable = this;
            OnInteractionStarted?.Invoke(this.transform);

            if (_onPuzzleRequested != null)
            {
                _onPuzzleRequested.Raise(_puzzleDefinition);
            }
        }

        public void HandlePuzzleSolved()
        {
            OnPuzzleSolved?.Invoke();
            // Optional: disable interaction after solved so they can't replay it
            // GetComponent<Collider>().enabled = false;
            // enabled = false;
        }

        public void HandlePuzzleCanceled()
        {
            OnPuzzleCanceled?.Invoke();
        }

        public string GetPromptText()
        {
            return _promptText;
        }

        public void SetPlayerInRange(bool inRange)
        {
            _playerInRange = inRange;
            
            if (inRange && _vfxSparkPrefab != null)
            {
                _activeVFX = Instantiate(_vfxSparkPrefab, transform.position + Vector3.up, Quaternion.identity);
                // Optionally child it to the object if it should follow
                // _activeVFX.transform.SetParent(transform);
            }
            else if (!inRange && _activeVFX != null)
            {
                Destroy(_activeVFX);
                _activeVFX = null;
            }
        }
    }
}
