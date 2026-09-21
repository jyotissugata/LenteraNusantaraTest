using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Sprinting")]
    public class IsSprintingConditionSO : CharacterConditionSO
    {
        [SerializeField] bool isSprinting = true;

        public override bool Evaluate(CharacterStateMachine machine)
        {
            return machine.IsSprinting == isSprinting;
        }
    }
}