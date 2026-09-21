using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Not Moving")]
    public class IsNotMovingConditionSO : CharacterConditionSO
    {
        public override bool Evaluate(CharacterStateMachine machine)
        {
            return machine.MoveInput.magnitude < 0.1f;
        }
    }
}