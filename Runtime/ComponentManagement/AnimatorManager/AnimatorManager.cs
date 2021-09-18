using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [System.Serializable]
    public class AnimatorState
    {
        public string stateName;
        public int layer;
    }

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

        #region Animation Playing

        /// <summary>
        /// Given a state name and a layer, try to play that state.
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
        /// Play animation from a state contained in a AnimatorStateManagerMono. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void PlayFromMonoData(AnimatorManagerStateMono mono) 
        {
            Play(mono.Value.stateName, mono.Value.layer);
        }

        /// <summary>
        /// Play animation from a state contained in a AnimatorStateManagerMono. This is used for compatibility with unity's graphic interface.
        /// </summary>
        public void PlayFromScriptableData(AnimatorManagerStateScriptable scriptable)
        {
            Play(scriptable.Value.stateName, scriptable.Value.layer);
        }

        #endregion
    }
}
