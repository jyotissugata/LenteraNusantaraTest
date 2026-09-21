using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Run")]
    public class RunStateSO : CharacterStateSO
    {
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private string _animatorFloatParam = "Speed";
        [SerializeField] private float _animatorSpeedValue = 1f;

        public override void OnEnter(CharacterStateMachine machine)
        {
            if (machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeed);

            if (machine.Animator != null)
                machine.Animator.SetFloat(_animatorFloatParam, _animatorSpeedValue);
        }
    }
}