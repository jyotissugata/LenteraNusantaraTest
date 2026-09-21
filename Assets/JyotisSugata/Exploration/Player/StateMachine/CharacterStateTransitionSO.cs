using System.Collections.Generic;
using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    [CreateAssetMenu(menuName = "JyotisSugata/StateMachine/Transition")]
    public class CharacterStateTransitionSO : ScriptableObject
    {
        [SerializeField] private List<CharacterConditionSO> _conditions = new List<CharacterConditionSO>();
        [SerializeField] private CharacterStateSO _targetState;

        public CharacterStateSO TargetState => _targetState;

        public bool IsMet(CharacterStateMachine machine)
        {
            if (_conditions == null || _conditions.Count == 0) return false;
            
            foreach (var condition in _conditions)
            {
                if (condition != null && !condition.Evaluate(machine))
                {
                    return false; // All conditions must be met (AND logic)
                }
            }
            return true;
        }
    }
}