using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    /// <summary>
    /// A state for one-shot animations (interact, attack, dash).
    /// Fires a Trigger on the Animator, waits for the animation to finish,
    /// then automatically transitions to a follow-up state.
    ///
    /// Two detection modes:
    /// 1. If Animation State Name is set: waits until animator ENTERS then EXITS
    ///    that state — works correctly whether or not the Animator has its own exit transition.
    /// 2. If Animation State Name is empty: falls back to normalizedTime >= 1 check.
    /// A safety Max Duration is always active to prevent infinite stuck.
    /// </summary>
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Action (One-Shot)")]
    public class ActionStateSO : CharacterStateSO
    {
        [Header("Animation")]
        [SerializeField] private string _animationTrigger = "Interact";
        [Tooltip("Must match the Animator State name exactly (case-sensitive). Leave empty to use normalizedTime fallback.")]
        [SerializeField] private string _animationStateName = "";
        [SerializeField] private int _animatorLayerIndex = 0;

        [Header("Movement during action")]
        [SerializeField] private bool _lockMovement = true;
        [SerializeField] private float _moveSpeedDuringAction = 0f;

        [Header("After action ends")]
        [SerializeField] private CharacterStateSO _nextState;
        [Tooltip("Safety fallback: force-exit this action state after this many seconds, even if animation detection fails.")]
        [SerializeField] private float _maxDuration = 5f;

        // Runtime-only — [System.NonSerialized] prevents saving to the SO asset.
        [System.NonSerialized] private int _entryFrame;
        [System.NonSerialized] private float _timeInState;
        [System.NonSerialized] private bool _hasEnteredTargetState;

        public override void OnEnter(CharacterStateMachine machine)
        {
            _entryFrame = Time.frameCount;
            _timeInState = 0f;
            _hasEnteredTargetState = false;

            if (_lockMovement && machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeedDuringAction);

            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.SetTrigger(_animationTrigger);
        }

        public override void OnUpdate(CharacterStateMachine machine)
        {
            if (machine.Animator == null) return;

            _timeInState += Time.deltaTime;

            // Wait 2 frames for the trigger to be picked up by the Animator.
            if (Time.frameCount <= _entryFrame + 2) return;

            bool isDone = false;

            if (!string.IsNullOrEmpty(_animationStateName))
            {
                var stateInfo = machine.Animator.GetCurrentAnimatorStateInfo(_animatorLayerIndex);
                bool isInTargetState = stateInfo.IsName(_animationStateName);
                bool isInTransition  = machine.Animator.IsInTransition(_animatorLayerIndex);

                if (!_hasEnteredTargetState)
                {
                    // Phase 1: wait until we are inside the target animation state.
                    if (isInTargetState)
                        _hasEnteredTargetState = true;
                    // If we never enter (wrong state name / already exited), timeout saves us.
                }
                else
                {
                    // Phase 2: done when:
                    // (a) Animator exited on its own via its own transition, OR
                    // (b) Animation reached its end with no Animator-side exit transition.
                    bool exitedByAnimator = !isInTargetState && !isInTransition;
                    bool endedInPlace     =  isInTargetState && stateInfo.normalizedTime >= 1f && !isInTransition;
                    isDone = exitedByAnimator || endedInPlace;
                }
            }
            else
            {
                // No state name provided: plain normalizedTime check.
                isDone = machine.IsActionAnimationFinished(_animatorLayerIndex);
            }

            // Safety timeout — always exits even if detection failed.
            if (_maxDuration > 0f && _timeInState >= _maxDuration)
                isDone = true;

            if (isDone)
            {
                CharacterStateSO target = _nextState ?? machine.FallbackState;
                if (target != null)
                    machine.TransitionTo(target);
            }
        }

        public override void OnExit(CharacterStateMachine machine)
        {
            // Clear trigger in case the animation didn't consume it yet.
            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.ResetTrigger(_animationTrigger);
        }
    }
}