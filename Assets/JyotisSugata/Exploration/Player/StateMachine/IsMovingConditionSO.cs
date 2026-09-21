using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Moving")]
    public class IsMovingConditionSO : CharacterConditionSO
    {
        [SerializeField] bool isMoving = true;

        public override bool Evaluate(CharacterStateMachine machine)
        {
            return (isMoving) == (machine.MoveInput.magnitude >= 0.1f);
        }
    }
}