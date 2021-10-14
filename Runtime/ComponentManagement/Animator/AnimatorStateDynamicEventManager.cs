using DoubleDash.CodingTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleDash.CodingTools.DebugTools;
using System.Linq;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class AnimatorStateDynamicEventManager : MonoBehaviour
    {
        [HelpBox("For this component to work, a single animation state's Behaviour Events must call this component's 'OnAnimationEnter', 'OnAnimationUpdate', and 'OnAnimationExit'. ")]

        //public int decimalPrecision =

        [SerializeField]
        List<AnimatorDynamicEvent> injectedEvents;

        [Header("Debug")]
        [SerializeField, ReadOnly, Tooltip("Current stored time of the animation.")]
        float lastStoredTime;
        [SerializeField, ReadOnly, Tooltip("Last stored index of the list of events.")]
        int lastStoredIndex;

        float stateLength;

        readonly TargetAnimatorState targetAnimatorState;

        public float firstFrameThreshold = 0.030f;
        public int decimalPrecision = 3;

        List<AnimatorDynamicEventRuntime> _runtimeEvents = new List<AnimatorDynamicEventRuntime>();

        List<AnimatorDynamicEventRuntime> RuntimeEvents 
        {
            get 
            {
                if (injectedEvents != null) 
                {
                    foreach (AnimatorDynamicEvent currentEvent in injectedEvents)
                        AddEventInternal(currentEvent);

                    //Clear
                    injectedEvents = null;

                    //After all events have been added, recalculate the starting index
                    if (lastStoredIndex >= 0) lastStoredIndex = GetIndexForTime(lastStoredTime);
                }

                return _runtimeEvents;
            }
        }

        #region Classes

        /// <summary>
        /// This class is used to debug if multiple animator states are trying to use the same Event Manager.
        /// </summary>
        [System.Serializable]
        public class TargetAnimatorState 
        {
            [SerializeField, ReadOnly, Tooltip("Animator using this dynamic event manager.")] 
            Animator animator;

            [SerializeField, ReadOnly, Tooltip("Animator state using this manager.")]
            int animStateHash;
        }

        /// <summary>
        /// This class determines during runtime if a single animator dynamic event has already been called during this iteration of the animation.
        /// </summary>
        class AnimatorDynamicEventRuntime
        {
            public bool complete;

            public AnimatorDynamicEvent baseEvent;

            public AnimatorDynamicEventRuntime(AnimatorDynamicEvent baseEvent)
            {
                complete = false;
                this.baseEvent = baseEvent;
            }
        }

        /// <summary>
        /// This class is used for inspector injection of dynamic events
        /// </summary>
        [System.Serializable]
        public class AnimatorDynamicEvent 
        {
            [Tooltip("Time during the animation at which the event occours.")]
            public float time;

            [Tooltip("Event triggered when the normalized time reaches the time value set above.")]
            public VariableReference<IGameEvent<DelegateEventSubscriber>> onEvent;

            public AnimatorDynamicEvent(float time, IGameEvent<DelegateEventSubscriber> onEvent)
            {
                this.time          = time;
                this.onEvent.Value = onEvent;
            }
        }

        #endregion

        #region Add Events

        public void AddEvent(AnimatorDynamicEvent newEvent) 
        {
            AddEventInternal(newEvent);

            //When a new event is added, the last index must be recalculated if it has been used
            if (lastStoredIndex >= 0) lastStoredIndex = GetIndexForTime(lastStoredTime);

        }

        void AddEventInternal(AnimatorDynamicEvent newEvent) 
        {
            for (int i = 0; i < _runtimeEvents.Count; i ++) 
            {

                //Insert event before another event that will take place at a later time
                if (newEvent.time < _runtimeEvents[i].baseEvent.time) 
                {
                    _runtimeEvents.Insert(i, new AnimatorDynamicEventRuntime(newEvent));
                    return;
                }
            }

            //Add at last
            _runtimeEvents.Add(new AnimatorDynamicEventRuntime(newEvent));
        }

        #endregion

        //Gets the time of an animation state without multipliers. 
        float GetCurrentUnmodifiedTime(AnimatorStateInfo animatorStateInfo)
        {
            //Get current normalized time for the animation
            float normalizedTime = animatorStateInfo.normalizedTime;

            //Get the current time of the animation. Multiply by the speed multiplier to get the base unmodified length
            float currentTime = normalizedTime * (animatorStateInfo.length * animatorStateInfo.speedMultiplier);

            //Avoid errors where the animator for some reason starts an animation farther into the first frame.
            if (currentTime < firstFrameThreshold) currentTime = 0;

            return currentTime;
        }

        //Gets the length of an animation state without multipliers. 
        float GetCurrentUnmodifiedLength(AnimatorStateInfo animatorStateInfo)
        {
            return animatorStateInfo.length * animatorStateInfo.speedMultiplier;
        }

        /// <summary>
        /// Checks all events that have passed since the last update
        /// </summary>
        /// <param name="animatorStateInfo"></param>
        void CheckEvents(AnimatorStateInfo animatorStateInfo)
        {
            //Get the current time of the animation;
            float currentTime = GetCurrentUnmodifiedTime(animatorStateInfo);

            //Compare with the last stored time
            float deltaTime = currentTime - lastStoredTime;

            //Get indexes of all events that must be called in order
            List<int> eventIndexes = GetIndexesForDeltaTime(lastStoredTime, deltaTime, animatorStateInfo.length * animatorStateInfo.speedMultiplier, animatorStateInfo);

            //For each event, trigger that event
            foreach (int eventIndex in eventIndexes) 
                RuntimeEvents[eventIndex].baseEvent.onEvent.Value.Trigger();

            /*
            //If there were indexes called
            if (eventIndexes.Count > 0) 
            {
                //Update last index
                lastStoredIndex = eventIndexes[eventIndexes.Count - 1];

                //reset event indexes
                eventIndexes.Clear();
            }
            */

            //Update the last stored time
            lastStoredTime = currentTime;
        }

        //Given an index, attempts to find the index of the last event that would have been called.
        int GetIndexForTime(float time) 
        {
            int i = 0;

            //Check each event to find an event that still wasn't meant to happen
            for (; i < RuntimeEvents.Count; i++) 
                if ((time < RuntimeEvents[i].baseEvent.time) || (time == 0 && RuntimeEvents[i].baseEvent.time == 0))
                    break;

            return i;
        }

        int ClampListIndex<T>(List<T> list, int index) 
        {
            if (index >= 0) return index % list.Count;
            else            return (list.Count + (index % list.Count));
        }

        //Gets all indexes for events from the starting time up to the ending time
        List<int> GetIndexesForDeltaTime(float startingTime, float deltaTime, float maximumTime, AnimatorStateInfo animatorStateInfo) 
        {
            //List of indexes that will be called
            List<int> indexes = new List<int>();

            //If the list of events is empty, stop
            if (RuntimeEvents.Count <= 0) return indexes;

             //Last time checkpoint
            float currentTime = startingTime;
            float targetTime  = startingTime + deltaTime;

            float currentUnmodifiedLength = GetCurrentUnmodifiedLength(animatorStateInfo);

            int currentLoop = (int) (startingTime / currentUnmodifiedLength);

            //Get index of last stamp
            float lastIndexTimeStamp = RuntimeEvents[lastStoredIndex].baseEvent.time;

            //int index = ClampListIndex(RuntimeEvents, lastStoredIndex);

            if (deltaTime >= 0)
            {
                while (currentTime <= targetTime) //Moving forward
                {

                    //Check if hasn't passed event
                    if (currentTime - (currentLoop * currentUnmodifiedLength) >= RuntimeEvents[lastStoredIndex].baseEvent.time)
                    {
                        //Get event
                        indexes.Add(lastStoredIndex);

                        //Update Index
                        lastStoredIndex += 1;

                        //Mark loop and loop forward
                        if (lastStoredIndex >= RuntimeEvents.Count)
                        {
                            //Check end
                            if (!animatorStateInfo.loop) 
                            {
                                currentTime = targetTime;
                                return indexes;
                            }

                            currentLoop += 1;
                            lastStoredIndex = 0;
                        }
                    }

                    //Update current time
                    currentTime = RuntimeEvents[lastStoredIndex].baseEvent.time + (currentLoop * currentUnmodifiedLength);

                }
            }
            else if (deltaTime < 0) 
            {
                while (currentTime >= targetTime) //Moving in reverse
                {
                    lastStoredIndex -= 1;

                    //Mark loop and loop back
                    if (lastStoredIndex <= 0)
                    {
                        currentLoop += 1;
                        lastStoredIndex = RuntimeEvents.Count - 1;
                    }

                    indexes.Add(lastStoredIndex);

                    //Update current time
                    currentTime = RuntimeEvents[lastStoredIndex].baseEvent.time - (currentLoop * currentUnmodifiedLength);

                }
            }

            //Set current time
            currentTime = targetTime;

            //Return the list of events
            return indexes;

            /*


            //For all events remaining (clamping the last index to non negative in case of a nonititialized index)
            for (int i = Mathf.Max(lastStoredIndex, 0); i < RuntimeEvents.Count; i++)
            {
                //If the elapsed time would have gone past or become equal the next event's time
                if (startingTime + deltaTime >= RuntimeEvents[i].baseEvent.time)
                    indexes.Add(i);
                else
                    //If the next event's time is greater than the final time, stop.
                    break;
            }






            //Check cases where the animation is moving forward with time
            if (animatorStateInfo.speedMultiplier > 0) 
            {
                //Check cases where the animation hasn't looped back
                if (deltaTime >= 0)
                {

                }
                else if (animatorStateInfo.loop && deltaTime < 0) //Check cases where the animator finished the animation and looped back
                {
                    //Get actual delta time from loop
                    deltaTime = maximumTime + deltaTime;

                    //Get initial index
                    int baseIndex = Mathf.Max(lastStoredIndex, 0);

                    //For all events remaining (clamping the last index to non negative in case of a nonititialized index)
                    for (int i = 0; i < RuntimeEvents.Count; i++)
                    {
                        int currentIndex = baseIndex + i;

                        float targetTime      = RuntimeEvents[i].baseEvent.time;

                        //Check loop
                        if (currentIndex >= RuntimeEvents.Count) 
                            targetTime += maximumTime;

                        //If the elapsed time would have gone past or become equal the next event's time
                        if (startingTime + deltaTime >= targetTime)
                            indexes.Add(i);
                        else
                            //If the next event's time is greater than the final time, stop.
                            break;
                    }
                }
            }

            //Return the list of events
            return indexes;
            */
        }

        #region Event Receivers

        public void OnAnimationEnter(AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters) 
        {
            //Set the last stored time to the exact time at which the animation entered
            lastStoredTime = GetCurrentUnmodifiedTime(parameters.animatorStateInfo);

            //Truncate last stored time to avoid errors
            if (lastStoredTime < (1f / Mathf.Pow(10, decimalPrecision))) lastStoredTime = 0;

            //Reset the last stored index so that it may be generated
            lastStoredIndex = GetIndexForTime(lastStoredTime);

            //Update events to check events that trigger on 0 seconds
            CheckEvents(parameters.animatorStateInfo);
        }

        public void OnAnimationUpdate(AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters)
        {
            //Update Events
            CheckEvents(parameters.animatorStateInfo);
        }

        public void OnAnimationExit(AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters)
        {
            //Update event one last time before leaving
            CheckEvents(parameters.animatorStateInfo);

            //Clear the last stored index
            lastStoredIndex = -1;
        }

        #endregion
    }
}
