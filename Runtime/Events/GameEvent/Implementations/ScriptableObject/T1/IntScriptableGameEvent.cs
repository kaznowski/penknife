using System;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleDash.CodingTools.GameEvents
{
    [CreateAssetMenu(fileName = "NewIntGameEvent", menuName = "DoubleDash/GameEvents/int GameEvent")]
    public class IntScriptableGameEvent : ScriptableGameEvent<int>
    {
        public override IGameEvent<DelegateEventSubscriber<int>> Value { get => _event; set => _event = (MultiStepGameEvent<int>) value; }
    }
}