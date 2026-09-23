using System.Collections.Generic;

using UnityEngine;

namespace JyotisSugata.Exploration.Player.StateMachine
{
    public abstract class CharacterStateSO : ScriptableObject
    {
        [SerializeField] private List<CharacterStateTransitionSO> _transitions = new List<CharacterStateTransitionSO>();

        public virtual void OnEnter(CharacterStateMachine machine) { }
        public virtual void OnUpdate(CharacterStateMachine machine) { }
        public virtual void OnExit(CharacterStateMachine machine) { }

        public CharacterStateSO CheckTransitions(CharacterStateMachine machine)
        {
            foreach (var transition in _transitions)
            {
                if (transition.IsMet(machine))
                {
                    return transition.TargetState;
                }
            }
            return null;
        }
    }
}
