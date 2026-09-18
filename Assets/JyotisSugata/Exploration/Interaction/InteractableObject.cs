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

        private GameObject _activeVFX;
        private bool _playerInRange = false;

        public void Interact()
        {
            if (_puzzleDefinition == null)
            {
                Debug.LogWarning($"PuzzleDefinition is missing on {gameObject.name}!");
                return;
            }
            
            if (_onPuzzleRequested != null)
            {
                _onPuzzleRequested.Raise(_puzzleDefinition);
            }
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
