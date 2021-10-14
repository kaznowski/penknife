using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DoubleDash.ComponentManagement.UIManagement.CanvasGroupManager;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.UIManagement
{

    public class CanvasDeactivationEvent : MonoBehaviour, IVariable<IGameEvent<DelegateEventSubscriber<CanvasGroupDeactivationData>, CanvasGroupDeactivationData>>
    {
        [SerializeField]
        CanvasDeactivationEvent.Event _canvasDeactivationEvent;

        public IGameEvent<DelegateEventSubscriber<CanvasGroupDeactivationData>, CanvasGroupDeactivationData> Value { get => _canvasDeactivationEvent; set => throw new System.NotImplementedException(); }

        [System.Serializable]
        public class Event : MultiStepGameEvent<CanvasGroupDeactivationData>
        {
        }
    }
}