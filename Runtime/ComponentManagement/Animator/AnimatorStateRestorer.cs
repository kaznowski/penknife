using DoubleDash.CodingTools;
using DoubleDash.CodingTools.DebugTools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace DoubleDash.ComponentManagement.AnimatorManagement {

    [RequireComponent(typeof(Animator))]
    public class AnimatorStateRestorer : MonoBehaviour
    {
        [Header("Debug")]

        [HelpBox("For this component to work, it must be placed above the animator in the Inspector Hierarchy.")]

        [SerializeField, ReadOnly, Tooltip("Is this component currently storing any animator data?")]
        bool storedData;

        [ReadOnly, Tooltip("What animations are currently being stored for each layer?")]
        List<AnimatorStateInfo> animatorCurrentAnimation = new List<AnimatorStateInfo>();

        [ReadOnly, Tooltip("For each animation, what is their current normalized time?")]
        List<float> animatorCurrentAnimationNormalizedTime = new List<float>();

        [ReadOnly, Tooltip("What parameters are available and what are their states?")]
        List<AnimatorControllerParameter> animatorParameters = new List<AnimatorControllerParameter>();

        //Internal animator component reference
        Animator _animatorComponent;

        public Animator AnimatorComponent
        {
            get
            {
                if (_animatorComponent == null) _animatorComponent = GetComponent<Animator>();
                return _animatorComponent;
            }
        }

        #region MonoBehaviour

        void OnEnable()
        {
            //Restore all animator data
            RestoreAnimator();
        }

        void OnDisable()
        {
            //Store all animator data
            StoreAnimator();
        }

        #endregion

        #region Private Functions

        //Stores all animator data
        void StoreAnimator()
        {
            //Begin storing
            StoreAnimatorParameters();
            StoreCurrentAnimationForEachLayer();

            //Tag data storage as complete
            storedData = true;
        }

        void StoreCurrentAnimationForEachLayer ()
        {
            //Clear the lists prior to storing
            animatorCurrentAnimation.Clear();
            animatorCurrentAnimationNormalizedTime.Clear();

            //Add the current state for each layer
            for (int i = 0; i < AnimatorComponent.layerCount; i++)
            {
                //Get info for that layer
                AnimatorStateInfo currentInfo = AnimatorComponent.GetCurrentAnimatorStateInfo(0);

                //Store values
                animatorCurrentAnimation.Add(currentInfo);
                animatorCurrentAnimationNormalizedTime.Add(currentInfo.normalizedTime);
            }
        }

        //Restores all animator data
        void RestoreAnimator()
        {
            //Do nothing if there is no stored data
            if (!storedData) 
                return;
            
            //Restore parameters and play the current stored animations for each layer
            RestoreAnimatorParameters();

            //Restore the animation for each layer;
            PlayStoredAnimationsForEachLayer();

            //Tag 
            storedData = false;
        }

        void PlayStoredAnimationsForEachLayer() 
        {
            for (int i = 0; i < animatorCurrentAnimation.Count; i++) 
            {
                AnimatorComponent.Play(animatorCurrentAnimation[i].fullPathHash, i, animatorCurrentAnimationNormalizedTime[i]);
            }
        }

        void StoreAnimatorParameters()
        {
            //Clear the list prior to storing
            animatorParameters.Clear();

            //Add each parameter
            for (int i = 0; i < AnimatorComponent.parameters.Length; i++)
                animatorParameters.Add(AnimatorComponent.parameters[i]);
        }

        //Restores the parameters
        void RestoreAnimatorParameters()
        {
            //Restore each parameter. The "Min" function here is making sure there is no out of bounds for any reason
            for (int i = 0; i < Mathf.Min(animatorParameters.Count, AnimatorComponent.parameters.Length); i++)
                AnimatorComponent.parameters[i] = animatorParameters[i];
        }

        #endregion
    }
}

