using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Idle")]
    public class IdleStateSO : CharacterStateSO
    {
        [SerializeField] private float _moveSpeed = 0f;
        [SerializeField] private string _animatorFloatParam = "Speed";
        [SerializeField] private float _animatorSpeedValue = 0f;

        public override void OnEnter(CharacterStateMachine machine)
        {
            if (machine.PlayerController != null)
                machine.PlayerController.SetMoveSpeed(_moveSpeed);
                
            if (machine.Animator != null)
                machine.Animator.SetFloat(_animatorFloatParam, _animatorSpeedValue);
        }
    }
}