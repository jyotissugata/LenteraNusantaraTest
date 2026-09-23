using UnityEngine;

using JyotisSugata.Core.StateMachine;

namespace JyotisSugata.Core.Events
{
    [CreateAssetMenu(menuName = "JyotisSugata/Events/GameState Event Channel")]
    public class GameStateEventChannel : EventChannelBase<GameStateType>
    {
    }
}
