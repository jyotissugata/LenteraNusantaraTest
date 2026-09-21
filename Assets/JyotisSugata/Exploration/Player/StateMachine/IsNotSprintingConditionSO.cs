using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Not Sprinting")]
    public class IsNotSprintingConditionSO : CharacterConditionSO
    {
        public override bool Evaluate(CharacterStateMachine machine)
        {
            return !machine.IsSprinting;
        }
    }
}