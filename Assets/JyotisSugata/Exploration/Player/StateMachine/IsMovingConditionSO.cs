using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Moving")]
    public class IsMovingConditionSO : CharacterConditionSO
    {
        [Tooltip("If true, condition passes when NOT moving (inverse check).")]
        [SerializeField] private bool _inverse = false;

        public override bool Evaluate(CharacterStateMachine machine)
        {
            bool isMoving = machine.MoveInput.magnitude >= 0.1f;
            return _inverse ? !isMoving : isMoving;
        }
    }
}
