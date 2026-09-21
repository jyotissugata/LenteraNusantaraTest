using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/States/Walk")]
    public class WalkStateSO : CharacterStateSO
    {
        [SerializeField] private float _moveSpeed = 4f;
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