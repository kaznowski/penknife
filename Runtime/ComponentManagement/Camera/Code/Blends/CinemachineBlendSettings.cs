using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    [System.Serializable]
    public class CinemachineBlendSettingsObject
    {
        [Tooltip("The style of the transition determines a curve that interpolates between Virtual Cameras over time.")]
        public CinemachineBlendDefinition.Style style;

        [Tooltip("Curve for when the style is set to Custom.")]
        public AnimationCurve customCurve = AnimationCurve.Linear(0,0,1,1);

        [Tooltip("Time it takes to make the transition. If the style is 'Cut' then the transition is immediate.")]
        public float timeInSeconds = 1;

        public CinemachineBlendDefinition GetDefinition
        {
            get
            {
                CinemachineBlendDefinition definition = new CinemachineBlendDefinition(style, timeInSeconds)
                {
                    m_CustomCurve = customCurve
                };
                return definition;
            }

        }
    }
}

