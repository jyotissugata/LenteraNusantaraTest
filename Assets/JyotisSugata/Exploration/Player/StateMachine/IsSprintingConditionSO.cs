using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Conditions/Is Sprinting")]
    public class IsSprintingConditionSO : CharacterConditionSO
    {
        [Tooltip("If true, condition passes when NOT sprinting (inverse check).")]
        [SerializeField] private bool _inverse = false;

        public override bool Evaluate(CharacterStateMachine machine)
        {
            return _inverse ? !machine.IsSprinting : machine.IsSprinting;
        }
    }
}