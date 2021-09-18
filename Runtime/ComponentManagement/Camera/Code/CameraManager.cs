using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.CinemachineManagement 
{

    [RequireComponent(typeof(Camera), typeof(CinemachineBrain))]
    public class CameraManager : MonoBehaviour
    {
        Camera _targetCamera;
        CinemachineBrain _targetCinemachineBrain;

        public Camera TargetCamera 
        {
            get 
            {
                if (_targetCamera == null) 
                    _targetCamera = GetComponent<Camera>();

                return _targetCamera;
            }
        }

        public CinemachineBrain TargetCinemachineBrain
        {
            get
            {
                if (_targetCinemachineBrain == null)
                    _targetCinemachineBrain = GetComponent<CinemachineBrain>();

                return _targetCinemachineBrain;
            }
        }

        public void SetDefaultBlendFromMono(CinemachineBlendSettingsMono mono)
        {
            TargetCinemachineBrain.m_DefaultBlend = mono.Value.GetDefinition;
        }

        public void SetDefaultBlendFromScriptable(CinemachineBlendSettingsScriptable scriptable)
        {
            TargetCinemachineBrain.m_DefaultBlend = scriptable.Value.GetDefinition;
        }

        public void SetDefaultBlend(CinemachineBlendDefinition blend) 
        {
            TargetCinemachineBrain.m_DefaultBlend = blend;
        }

        public void SetDefaultBlendTime(float time)
        {
            TargetCinemachineBrain.m_DefaultBlend.m_Time = time;
        }

        public void SetCustomBlendSettings(CinemachineBlenderSettings blenderSettings) 
        {
            TargetCinemachineBrain.m_CustomBlends = blenderSettings;
        }
    }
}


