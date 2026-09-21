using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    public abstract class CharacterConditionSO : ScriptableObject
    {
        public abstract bool Evaluate(CharacterStateMachine machine);
    }
}