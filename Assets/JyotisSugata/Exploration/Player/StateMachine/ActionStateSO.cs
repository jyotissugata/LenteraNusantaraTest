using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    /// <summary>
    /// A state for one-shot animations (interact, attack, dash).
    /// Fires a Trigger on the Animator, waits for the animation to finish,
    /// then automatically transitions to a follow-up state.
    /// Unlike loop states, this does NOT use transitions in the list,
    /// and it blocks AnyState transitions while active.
    /// </summary>
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Action (One-Shot)")]
    public class ActionStateSO : CharacterStateSO
    {
        [Header("Animation")]
        [SerializeField] private string _animationTrigger = "Interact";
        [SerializeField] private string _animationStateName = "Interact"; // Must match the Animator state name exactly
        [SerializeField] private int _animatorLayerIndex = 0;

        [Header("Movement during action")]
        [SerializeField] private bool _lockMovement = true;
        [SerializeField] private float _moveSpeedDuringAction = 0f;

        [Header("After action ends")]
        [SerializeField] private CharacterStateSO _nextState;

        // Runtime flag — tracks whether the animator has entered the target state yet.
        // ScriptableObjects are shared assets, so we use [System.NonSerialized] to keep this per-play.
        [System.NonSerialized] private bool _hasEnteredAnimation;

        public override void OnEnter(CharacterStateMachine machine)
        {
            _hasEnteredAnimation = false;

            if (_lockMovement && machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeedDuringAction);

            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.SetTrigger(_animationTrigger);
        }

        public override void OnUpdate(CharacterStateMachine machine)
        {
            if (machine.Animator == null) return;

            // Step 1: Wait until the Animator has actually entered our target animation state.
            // This prevents an immediate exit on the first frame when normalizedTime is still 0.
            if (!_hasEnteredAnimation)
            {
                // If no state name specified, just wait one transition to be safe
                if (string.IsNullOrEmpty(_animationStateName))
                {
                    if (!machine.Animator.IsInTransition(_animatorLayerIndex))
                        _hasEnteredAnimation = true;
                }
                else
                {
                    var stateInfo = machine.Animator.GetCurrentAnimatorStateInfo(_animatorLayerIndex);
                    if (stateInfo.IsName(_animationStateName))
                        _hasEnteredAnimation = true;
                }
                return; // Don't check finish until we've confirmed entry
            }

            // Step 2: Check if animation has finished
            if (machine.IsActionAnimationFinished(_animatorLayerIndex))
            {
                CharacterStateSO target = _nextState ?? machine.FallbackState;
                if (target != null)
                    machine.TransitionTo(target);
            }
        }

        public override void OnExit(CharacterStateMachine machine)
        {
            _hasEnteredAnimation = false;
            // Clear the trigger in case the animation didn't consume it yet
            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.ResetTrigger(_animationTrigger);
        }
    }
}