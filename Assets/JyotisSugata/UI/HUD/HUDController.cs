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

namespace JyotisSugata.UI.HUD
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private BoolEventChannel _onInteractionPromptChanged;
        [SerializeField] private HUDView _hudView;
        [SerializeField] private string _promptMessage = "Press [E] to Interact";

        private void OnEnable()
        {
            if (_onInteractionPromptChanged != null)
            {
                _onInteractionPromptChanged.Subscribe(HandlePromptChanged);
            }
        }

        private void OnDisable()
        {
            if (_onInteractionPromptChanged != null)
            {
                _onInteractionPromptChanged.Unsubscribe(HandlePromptChanged);
            }
        }

        private void HandlePromptChanged(bool show)
        {
            if (_hudView != null)
            {
                if (show)
                {
                    _hudView.ShowPrompt(_promptMessage);
                }
                else
                {
                    _hudView.HidePrompt();
                }
            }
        }
    }
}
