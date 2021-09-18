using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.CinemachineManagement
{
    public class CinemachineBlendSettingsMono : MonoBehaviour,
        IVariable<CinemachineBlendSettingsObject>
    {
        public CinemachineBlendSettingsObject _blendSettings;

        public CinemachineBlendSettingsObject Value { get => _blendSettings; set => _blendSettings = value; }
    }
}

