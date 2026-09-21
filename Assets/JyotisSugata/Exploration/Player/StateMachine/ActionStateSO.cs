using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    /// <summary>
    /// A state for one-shot animations (interact, attack, dash).
    /// Fires a Trigger on the Animator, waits for the animation to finish,
    /// then automatically transitions to a follow-up state.
    /// Unlike loop states, this does NOT use transitions in the list.
    /// </summary>
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Action (One-Shot)")]
    public class ActionStateSO : CharacterStateSO
    {
        [Header("Animation")]
        [SerializeField] private string _animationTrigger = "Interact";
        [SerializeField] private int _animatorLayerIndex = 0;

        [Header("Movement during action")]
        [SerializeField] private bool _lockMovement = true;
        [SerializeField] private float _moveSpeedDuringAction = 0f;

        [Header("After action ends")]
        [SerializeField] private CharacterStateSO _nextState;

        public override void OnEnter(CharacterStateMachine machine)
        {
            if (_lockMovement && machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeedDuringAction);

            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.SetTrigger(_animationTrigger);
        }

        public override void OnUpdate(CharacterStateMachine machine)
        {
            // Wait for the action animation to finish, then move to next state
            if (machine.IsActionAnimationFinished(_animatorLayerIndex))
            {
                CharacterStateSO target = _nextState;

                // If no explicit next state, pick Walk or Idle based on current input
                if (target == null)
                    target = machine.FallbackState;

                if (target != null)
                    machine.TransitionTo(target);
            }
        }

        public override void OnExit(CharacterStateMachine machine)
        {
            // Clear the trigger in case the animation didn't consume it yet
            if (machine.Animator != null && !string.IsNullOrEmpty(_animationTrigger))
                machine.Animator.ResetTrigger(_animationTrigger);
        }
    }
}