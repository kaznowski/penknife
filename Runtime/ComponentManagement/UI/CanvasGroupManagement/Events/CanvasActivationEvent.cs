using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DoubleDash.ComponentManagement.UIManagement.CanvasGroupManager;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.UIManagement {

    public class CanvasActivationEvent : MonoBehaviour, IVariable<IGameEvent<DelegateEventSubscriber<CanvasGroupActivationData>, CanvasGroupActivationData>>
    {
        [SerializeField]
        CanvasActivationEvent.Event _canvasActivationEvent;

        public IGameEvent<DelegateEventSubscriber<CanvasGroupActivationData>, CanvasGroupActivationData> Value { get => _canvasActivationEvent; set => throw new System.NotImplementedException(); }

        [System.Serializable]
        public class Event : MultiStepGameEvent<CanvasGroupActivationData>
        {
        }
    }
}

