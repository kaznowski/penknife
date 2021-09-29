using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [System.Serializable]
    public class AnimatorManagerState
    {
        [Tooltip("What is the name of this state on the animator?")]
        public string stateName = "New State";
        [Tooltip("At which layer is this animation?")]
        public string layer = "Base Layer";
    }
}