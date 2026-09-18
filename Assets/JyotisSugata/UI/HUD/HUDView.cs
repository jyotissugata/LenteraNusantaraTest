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
using TMPro;

namespace JyotisSugata.UI.HUD
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private GameObject _promptPanel;
        [SerializeField] private TextMeshProUGUI _promptText;

        public void ShowPrompt(string text)
        {
            if (_promptPanel != null)
            {
                _promptPanel.SetActive(true);
            }
            if (_promptText != null)
            {
                _promptText.text = text;
            }
        }

        public void HidePrompt()
        {
            if (_promptPanel != null)
            {
                _promptPanel.SetActive(false);
            }
        }
    }
}
