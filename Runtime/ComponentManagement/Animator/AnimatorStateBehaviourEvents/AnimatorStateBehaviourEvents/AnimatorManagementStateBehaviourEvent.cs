using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public abstract class AnimatorManagementStateBehaviourEvent : StateMachineBehaviour
    {
        [Header("References")]
        [Tooltip("This key is usually a scriptable object that is referenced both by this state, and the Animator that will play this state." + "\n\n" +
                 "It is used to map instances of this event to its respective animator Event Subscriber, calling that subscriber when needed.")]

        [SerializeField] AnimatorStateBehaviourKey _key;

        //Cached subscriber component for this object
        AnimatorEventSubscriber animatorEventSubscriberComponent;

        [Header("Settings")]

        [Tooltip("If this Behaviour can't find a subscriber, should it be disposed to save performance?")]
        public bool disposeIfWithoutSubscriber = false;
        [Tooltip("If the subscriber linked to this behaviour has no keys that match this StateBehaviour's keys, should it be disposed to save performance?")]
        public bool disposeIfWithoutKeys = false;

        [Tooltip("How should this behaviour be disposed of?")]
        public DisposeMethod disposeMethod = DisposeMethod.destroy;
        public enum DisposeMethod 
        { 
            destroy,
            //disable //Unity has added the option to toggle this to be disabled, but it isn't available at runtime or by code... Mecanim's last post about this was in 2015
        }

        #region Public Properties

        public AnimatorStateBehaviourKey Key => _key;

        #endregion

        #region Protected Functions

        protected void StateBehaviourTrigger(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Get the event subscriber from the animator
            GetAnimatorEventSubscriber(animator);

            //Error check for the Animator gameobject not having a subscriber
            if (animatorEventSubscriberComponent == null)
            {
                if (disposeIfWithoutSubscriber)
                {
                    Dispose();
                    return;
                }
            }
            else 
            {
                //Generate data for trigger
                AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters = new AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters(animator, stateInfo, layerIndex);

                //Trigger each of the KeyMaps
                bool triggeredAnyKeymaps = animatorEventSubscriberComponent.TriggerAllKeyMapsForKey(Key, parameters);

                //If there were no triggers, consider disposing
                if ((triggeredAnyKeymaps == false) && disposeIfWithoutKeys)
                    Dispose();
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Gets rid of this component
        /// </summary>
        void Dispose() {
            Destroy(this);
        }

        /// <summary>
        /// Gets the AnimatorEventSubscriber from the same gameObject as the Animator
        /// </summary>
        /// <param name="animator"></param>
        /// <returns></returns>
        void GetAnimatorEventSubscriber(Animator animator) 
        {
            //If the subscriber component isn't null, and its gameobject is the same as the animator's stop
            if (animatorEventSubscriberComponent != null)
                if ((animatorEventSubscriberComponent.gameObject == animator.gameObject))
                    return;

            //Otherwise, get it from the same GameObject as the Animator
            animatorEventSubscriberComponent = animator.gameObject.GetComponent<AnimatorEventSubscriber>();
        }

        #endregion
    }
}