using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorManager : MonoBehaviour
    {
        Animator _animatorComponent;

        public Animator AnimatorComponent 
        {
            get 
            {
                if (_animatorComponent == null) _animatorComponent = GetComponent<Animator>();
                return _animatorComponent;
            }
        }

        #region Animator - Play

        /// <summary>
        /// Given a state name and a layer index, try to play that state.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="layerIndex"></param>
        public void Play(string stateName, int layerIndex)
        {
            //Only play if the component is active and enabled
            if (!AnimatorComponent.isActiveAndEnabled) return;
            //Play from that state and layer
            AnimatorComponent.Play(stateName, layerIndex);
        }

        /// <summary>
        /// Given a state name and a layer name, try to play that state.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="layerIndex"></param>
        public void Play(string stateName, string layerName)
        {
            Play(stateName, AnimatorComponent.GetLayerIndex(layerName));
        }

        /// <summary>
        /// Play animation from a state contained in a AnimatorStateManagerMono. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void PlayFromMonoData(AnimatorManagerStateMono mono) 
        {
            Play(mono.Value.stateName, AnimatorComponent.GetLayerIndex(mono.Value.layer));
        }

        /// <summary>
        /// Play animation from a state contained in a AnimatorStateManagerMono. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void PlayFromScriptableData(AnimatorManagerStateScriptable scriptable)
        {
            Play(scriptable.Value.stateName, AnimatorComponent.GetLayerIndex(scriptable.Value.layer));
        }

        #endregion

        #region Animator - SetParameter

        /// <summary>
        /// Given settings of a Animator Parameter configuration, set that parameter.
        /// </summary>
        public void SetParameter(AnimatorManagerParameter parameter)
        {
            switch (parameter.parameterType)
            {
                case (AnimatorControllerParameterType.Bool):
                    AnimatorComponent.SetBool(parameter.parameterName, parameter.boolValue);
                    break;
                case (AnimatorControllerParameterType.Int):
                    AnimatorComponent.SetInteger(parameter.parameterName, parameter.intValue);
                    break;
                case (AnimatorControllerParameterType.Float):
                    AnimatorComponent.SetFloat(parameter.parameterName, parameter.floatValue);
                    break;
                case (AnimatorControllerParameterType.Trigger):
                    AnimatorComponent.SetTrigger(parameter.parameterName);
                    break;
            }
        }

        /// <summary>
        /// Set Parameter from a Animator Parameter Settings contained in a AnimatorManagerParameterMono. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void SetParameterFromMonoData(AnimatorManagerParameterMono parameter)
        {
            SetParameter(parameter.Value);
        }

        /// <summary>
        /// Set Parameter from a Animator Parameter Settings contained in a AnimatorManagerParameterScriptable. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void SetParameterFromScriptableData(AnimatorManagerParameterScriptable parameter)
        {
            SetParameter(parameter.Value);
        }

        #endregion
    }
}
