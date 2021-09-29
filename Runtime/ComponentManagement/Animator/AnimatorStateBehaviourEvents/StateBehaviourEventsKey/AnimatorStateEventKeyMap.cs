using DoubleDash.CodingTools;
using DoubleDash.ComponentManagement.AnimatorManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateEventKeyMap : MonoBehaviour, IVariable<AnimatorStateBehaviourKey>
{
    [SerializeField, SerializeProperty("Key")]
    AnimatorStateBehaviourKey _key;

    [Header("Events")]
    [Tooltip("Triggered when the Key for this Keymap is changed.")]
    public GameEvent<AnimatorStateBehaviourEventParameters> onTrigger = new GameEvent<AnimatorStateBehaviourEventParameters>();
    public GameEvent<KeyChange> onKeyChanged = new GameEvent<KeyChange>() ;

    #region Classes

    /// <summary>
    /// This class contains all data available to Animator State Behaviours
    /// </summary>
    public class AnimatorStateBehaviourEventParameters {
        public Animator animator;
        public AnimatorStateInfo animatorStateInfo;
        public int layerIndex;

        public AnimatorStateBehaviourEventParameters(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
        {
            this.animator = animator;
            this.animatorStateInfo = animatorStateInfo;
            this.layerIndex = layerIndex;
        }
    }

    /// <summary>
    /// This class contains the value of the key before and after a change.
    /// </summary>
    public class KeyChange
    {
        public AnimatorStateEventKeyMap keyMap;
        public AnimatorStateBehaviourKey oldKey;
        public AnimatorStateBehaviourKey newKey;

        public KeyChange(AnimatorStateEventKeyMap keyMap, AnimatorStateBehaviourKey oldKey)
        {
            this.keyMap = keyMap;
            this.oldKey = oldKey;
        }
    }

    #endregion

    #region Public Properties

    public AnimatorStateBehaviourKey Value 
    {
        get => _key;
        set 
        {
            //If change is redundant, do nothing
            if (value == _key) return;

            //Store old value
            AnimatorStateBehaviourKey oldKey = _key;

            //Set value and trigger event
            _key = value;
            onKeyChanged.Trigger(new KeyChange(this, oldKey));
        }
    }

    public AnimatorStateBehaviourKey Key 
    {
        get => Value;
        set => Value = value;
    }

    #endregion
}
