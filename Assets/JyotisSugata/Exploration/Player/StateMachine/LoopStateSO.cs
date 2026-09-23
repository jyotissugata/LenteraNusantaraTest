using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    /// <summary>
    /// General-purpose looping state. One script handles Idle, Walk, Run, Crouch, etc.
    /// Just create a separate SO asset for each and set the values in the Inspector.
    /// </summary>
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Loop State")]
    public class LoopStateSO : CharacterStateSO
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 4f;

        [Header("Animation")]
        [SerializeField] private string _animatorFloatParam = "Speed";
        [SerializeField] private float _animatorSpeedValue = 0.5f;

        public override void OnEnter(CharacterStateMachine machine)
        {
            if (machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeed);

            if (machine.Animator != null)
                machine.Animator.SetFloat(_animatorFloatParam, _animatorSpeedValue);
        }
    }
}
