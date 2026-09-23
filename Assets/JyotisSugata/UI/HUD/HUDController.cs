using UnityEngine;

using JyotisSugata.Core.Events;

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
