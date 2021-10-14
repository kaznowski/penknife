using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.UIManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupManager : MonoBehaviour, INameable
    {
        [Header("Settings")]
        [Tooltip("Time this canvas group takes to transition into.")]
        public float timeTransitionIn = 0.5f;
        [Tooltip("Time this canvas group takes to transition out from.")]
        public float timeTransitionOut = 0.5f;
        [Tooltip("If this is enabled, the canvas by default will not block raycasts, even in conditions where it normally would be.")]
        public bool neverBlocksRaycasts;
        [Tooltip("If this is enabled, the canvas by default will not be set as interactable, even in conditions where it normally would be.")]
        public bool neverInteractable;

        [Header("Default Settings")]
        public bool defaultActive;
        public bool defaultInteractable;

        [Header("Events")]

        [Tooltip("Triggered when this group has begun to activate.")]
        public VariableReference<IGameEvent<DelegateEventSubscriber<CanvasGroupActivationData>, CanvasGroupActivationData>> onBeginActivation;
        [Tooltip("Triggered when this group has finished activating.")]
        public VariableReference<IGameEvent<DelegateEventSubscriber<CanvasGroupActivationData>, CanvasGroupActivationData>> onEndActivation;

        [Tooltip("Triggered when this canvas group began deactivating.")]
        public VariableReference<IGameEvent<DelegateEventSubscriber<CanvasGroupDeactivationData>, CanvasGroupDeactivationData>> onBeginDeactivation;
        [Tooltip("Triggered when this canvas group has just finished deactivating.")]
        public VariableReference<IGameEvent<DelegateEventSubscriber<CanvasGroupDeactivationData>, CanvasGroupDeactivationData>> onEndDeactivation;

        [Header("Debug")]
        [ReadOnly] [SerializeField] bool _active = true;
        [ReadOnly] [SerializeField] bool _locked = false;
        [ReadOnly] [SerializeField] CanvasGroup _canvasGroup;

        public class CanvasGroupActivationData {

            //Time it will take before the canvas begins activating.
            readonly public float delayTime = 0;
            //Time it takes to activate the canvas after the delay.
            readonly public float activationTime = 0;
            //Will this canvas be activated, but not interactable?
            readonly public bool activateAsInteractable;

            public CanvasGroupActivationData(float totalTime, float timeTransitionIn, bool activateAsInteractable) 
            {
                this.activateAsInteractable = activateAsInteractable;

                //If the total time allows for delay, set the excess time as delay.
                if (totalTime >= timeTransitionIn)
                {
                    delayTime      = totalTime - timeTransitionIn;
                    activationTime = timeTransitionIn;
                }
                else //If the total time does not allow for delay, set the activation time as the transition time
                {
                    activationTime = totalTime;
                }
            }
        }

        public class CanvasGroupDeactivationData
        {
            //Time it takes to deactivate the canvas
            readonly public float deactivationTime = 0;

            public CanvasGroupDeactivationData(float totalTime, float timeTransitionOut)
            {
                //If the total time allows for delay, set the deactivation time as the transition out time.
                if (totalTime >= timeTransitionOut)
                {
                    deactivationTime = timeTransitionOut;
                }
                else //If the total time does not allow for delay, set the deactivation time as the total time allowed
                {
                    deactivationTime = totalTime;
                }
            }
        }

        public CanvasGroup Group {
            get 
            {
                //Get component if needed, and return it.
                if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        public bool IsActive {
            get => _active;
        }
        public bool IsLocked
        {
            get => _locked;
            set
            {
                //Set the value for locked
                _locked = value;

                // Unlocking
                if (_locked == true)
                {
                    Group.interactable   = false;
                    Group.blocksRaycasts = false;
                }
                else //Locking
                {
                    //Check if can block raycasts or be interactable
                    if (!neverBlocksRaycasts) Group.blocksRaycasts = true;
                    if (!neverInteractable)   Group.interactable   = true;
                }
            }
        }

        void Awake() {
            SetDefaultActivation();
        }

        void SetDefaultActivation() {
            if (defaultActive)
            {
                //Flip the tag to false to allow activation
                _active = false;
                ActivateGroup(0, defaultInteractable);
            }
            else
            {
                //Flip the tag to true to allow activation
                _active = true;
                DeactivateCanvasGroup(0);
            }
        }

        public void ActivateCanvasGroup(float totalTransitionTime) 
        {
            ActivateGroup(totalTransitionTime, true);
        }

        public void ActivateCanvasGroupOnBackground(float totalTransitionTime)
        {
            ActivateGroup(totalTransitionTime, false);
        }

        void ActivateGroup(float totalTransitionTime, bool activateAsInteractable) 
        {
            //If already active, ignore activation, but set the interactable status.
            if (IsActive) {
                //Set locked status
                IsLocked = !activateAsInteractable;
                //Stop
                return;
            }

            //Get data for the activation of this canvas
            CanvasGroupActivationData activationData = new CanvasGroupActivationData(totalTransitionTime, timeTransitionIn, activateAsInteractable);

            //Fire event for begin deactivation
            onBeginActivation.Value.Trigger(activationData);

            //Call corroutine for deactivation
            StartCoroutine(Activate(activationData));
        }

        public void DeactivateCanvasGroup(float totalTransitionTime)
        {
            //Lock
            IsLocked = true;

            //If already inactive, ignore.
            if (!IsActive) return;

            //Get data for the deactivation of this canvas
            CanvasGroupDeactivationData deactivationData = new CanvasGroupDeactivationData(totalTransitionTime, timeTransitionOut);

            //Fire event for begin deactivation
            onBeginDeactivation.Value.Trigger(deactivationData);

            //Call corroutine for deactivation
            StartCoroutine(Deactivate(deactivationData));
        }

        IEnumerator Deactivate(CanvasGroupDeactivationData deactivationData) 
        {
            //Wait until the required time to deactivate
            if(deactivationData.deactivationTime > 0) yield return new WaitForSeconds(deactivationData.deactivationTime);

            //Deactivate
            _active = false;
            _locked = true;

            //Fire event for deactivation
            onEndDeactivation.Value.Trigger(deactivationData);
        }

        IEnumerator Activate(CanvasGroupActivationData activationData) 
        {
            //Wait until the required delay time to activate
            if(activationData.delayTime > 0) yield return new WaitForSeconds(activationData.delayTime);

            //Fire event for ending the delay
            onBeginActivation.Value.Trigger(activationData);

            //Wait for the activation time
            if (activationData.activationTime > 0) yield return new WaitForSeconds(activationData.activationTime);

            //Activate
            _active = true;
            IsLocked = !activationData.activateAsInteractable;

            //Fire event for finishing activation
            onEndActivation.Value.Trigger();
        }


        public string Name { get => gameObject.name; set => gameObject.name = value; }
    }
}