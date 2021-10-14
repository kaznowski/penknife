using DoubleDash.CodingTools;
using DoubleDash.CodingTools.GenericDatabases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.ComponentManagement.UIManagement
{
    public class ControlCanvases : MonoBehaviour
    {
        public VariableReference<Database<CanvasGroupManager>> canvases;

        public VariableReference<StartingConfiguration> startingConfiguration;

        /*
        #region Public Properties

        public bool IsTransitioning 
        {
            get => _isTransitioning;
        }

        #endregion
        */

        #region Events

        [Header("Events")]

        [Tooltip("This event is fired when a call to the 'BeginTransition' method yields success.")]
        public GameEvent<ControlCanvasTransitionArgs> onTransitionStart;

        [System.Serializable]
        public class ControlCanvasTransitionArgs {
            [Tooltip("Transition to use when determining states of different canvases.")]
            public CanvasTransition transition;
        }

        #endregion

        /*
        #region Debug

        [Header("Debug")]

        [SerializeField, ReadOnly, Tooltip("Tag to determine if this control canvas is currently in a transition.")]
        bool _isTransitioning;

        [SerializeField, ReadOnly, Tooltip("Time to be elapsed until the current transition ends.")] 
        float transitionCooldownInSeconds;

        #endregion
        */

        [System.Serializable]
        public class StartingConfiguration 
        {
            [Tooltip("The following Canvas Group Managers will start as active.")]
            public VariableReference<Database<CanvasGroupManager>> startAsActive;

            [Tooltip("The following Canvas Group Managers will start as background. All other canvases in this ControlCanvas' database will start inactive.")]
            public VariableReference<Database<CanvasGroupManager>> startAsBackground;

            [Tooltip("The following transition will be played out as soon as all active and background Canvas Group Managers have been set.")]
            public CanvasTransition startingTransition;

            [Tooltip("The following value for delay (if not negative) will override the Transition's delay value.")]
            public float overrideDelay = -1f;

            [Tooltip("The following value for the transition time (if not negative) will override the Transition's transition time.")]
            public float overrideTransitionTime = -1f;
        }

        /// <summary>
        /// When this Component activates, the starting configuration is adjusts which objects should be set to active, background, or inactive.
        /// </summary>
        void Start()
        {
            SetStartingConfiguration(startingConfiguration.Value);
        }

        /// <summary>
        /// Uses a canvas transition object to set which canvas objects should remain active. Returns true if the transition successfuly began.
        /// </summary>
        public void BeginTransition(CanvasTransition transition)
        {
            BeginTransitionInternal(transition.transitionTimeInSeconds, transition.activeCanvases.ToArray(), transition.backgroundCanvases.ToArray());
        }

        /// <summary>
        /// Sets which canvases should start activated, and which should be transitioned into.
        /// </summary>
        /// <param name="configuration"></param>
        public void SetStartingConfiguration(StartingConfiguration configuration)
        {
            //Set all initial states as an immediate transition
            BeginTransitionInternal(0f, configuration.startAsActive.Value.GetAllEntriesAsArray(), configuration.startAsBackground.Value.GetAllEntriesAsArray());

            //Begin transitioning into the starting transition
            BeginTransition(configuration.startingTransition);
        }

        void BeginTransitionInternal(float transitionTimeInSeconds, CanvasGroupManager[] canvasesToStartAsActivated, CanvasGroupManager[] canvasesToStartAsBackground) 
        {
            //Cache database of all canvas group contained by this. These will be deactivated unless they are on the list of canvases that will be active or on background
            Dictionary<string, CanvasGroupManager> canvasesToStartAsDeactivated = canvases.Value.CloneDatabase();

            //Check the list of canvases that will start activated, removing items from the list of canvases that will start deactivated.
            //Canvases that will start activated are also immediatelly activated
            foreach (CanvasGroupManager canvas in canvasesToStartAsActivated)
            {
                //Remove from list of canvases that will start deactivated.
                if (canvasesToStartAsDeactivated.ContainsKey(canvas.Name)) canvasesToStartAsDeactivated.Remove(canvas.Name);

                //Immediatelly activate canvas that has to start activated.
                canvas.ActivateCanvasGroup(transitionTimeInSeconds);
            }

            //For each canvas that is meant to be on the background on the transition, activate that canvas while keeping buttons locked
            foreach (CanvasGroupManager canvas in canvasesToStartAsBackground)
            {
                //Remove from list of canvases that will start deactivated.
                if (canvasesToStartAsDeactivated.ContainsKey(canvas.Name)) canvasesToStartAsDeactivated.Remove(canvas.Name);

                //Immediatelly activate canvas that has to start activated.
                canvas.ActivateCanvasGroup(transitionTimeInSeconds);
            }

            //For each canvas that has remained on the list of canvas to be deactivated, deactivate it.
            foreach (KeyValuePair<string, CanvasGroupManager> pair in canvasesToStartAsDeactivated)
                pair.Value.DeactivateCanvasGroup(transitionTimeInSeconds);
        }
    }
}
