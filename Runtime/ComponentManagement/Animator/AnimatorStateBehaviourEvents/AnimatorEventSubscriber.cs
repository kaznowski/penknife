using DoubleDash.CodingTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DoubleDash.ComponentManagement.AnimatorManagement 
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventSubscriber : MonoBehaviour
    {
        //Animator component attached to this object
        Animator _animatorComponent;

        /// <summary>
        /// Animator Component attached to this GameObject.
        /// </summary>
        public Animator AnimatorComponent
        {
            get
            {
                if (_animatorComponent == null) _animatorComponent = GetComponent<Animator>();
                return _animatorComponent;
            }
        }

        [SerializeField, Tooltip("Entries to be added to this component via inspector editing.")] 
        List<AnimatorStateEventKeyMap> injectedKeyMaps = new List<AnimatorStateEventKeyMap>();

        //Dictionary of Keys to their Keymaps. A key map entry also carries an event that updates all objects that depend on that list of entries
        Dictionary<AnimatorStateBehaviourKey, List<KeyEntry>> _keysToMaps;

        public class KeyEntry 
        {
            //Reference to the Keymap
            public AnimatorStateEventKeyMap keyMap;

            //Event slot handle for that keymap's on key change
            public IEventSlotHandle onKeyChangeHandle;

            public KeyEntry(AnimatorStateEventKeyMap keyMap, IEventSlotHandle onKeyChangeHandle)
            {
                this.keyMap = keyMap;
                this.onKeyChangeHandle = onKeyChangeHandle;
            }
        }

        KeyEntry TryGetKeyMapEntry(List<KeyEntry> list, AnimatorStateEventKeyMap keyMap) 
        {
            for (int i = 0; i < list.Count; i++) 
            {
                if (list[i].keyMap == null)
                {
                    list.RemoveAt(i);
                    i--;
                }
                else 
                {
                    if (list[i].keyMap == keyMap)
                        return list[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the dictionary, initializing it if needed
        /// </summary>
        Dictionary<AnimatorStateBehaviourKey, List<KeyEntry>> KeysToMaps 
        {
            get 
            {
                //Initialize the dictionaries if they are null
                if (_keysToMaps == null) 
                    _keysToMaps = new Dictionary<AnimatorStateBehaviourKey, List<KeyEntry>>();

                //If there are injected entries, pass them to the dict
                if (injectedKeyMaps.Count > 0)
                    foreach (AnimatorStateEventKeyMap currentKeyMap in injectedKeyMaps)
                        InjectKeyMap(currentKeyMap);

                //Clear list of keymaps that had to be injected
                injectedKeyMaps.Clear();

                //return the dictionary
                return _keysToMaps;
            }
        }

        /// <summary>
        /// Injects the keymap into the dictionary.
        /// </summary>
        /// <param name="keyMap"></param>
        void InjectKeyMap(AnimatorStateEventKeyMap keyMap) 
        {
            //If key does not exist in dictionary, create it
            if (!_keysToMaps.ContainsKey(keyMap.Key))
                _keysToMaps.Add(keyMap.Key, new List<KeyEntry>());

            //If the keymap exists for that key, stop
            if (TryGetKeyMapEntry(_keysToMaps[keyMap.Key], keyMap) != null)
                return;

            //Subscribe to that keymap's change envent
            IEventSlotHandle newHandle = keyMap.onKeyChanged.Subscribe(OnKeyMapChange);

            //Generate Keymap Entry
            KeyEntry newEntry = new KeyEntry(keyMap, newHandle);

            //Add that specific keymap
            _keysToMaps[keyMap.Key].Add(newEntry);
        }

        /// <summary>
        /// Triggered by keymaps when their keys change.
        /// </summary>
        /// <param name="currentKey"></param>
        void OnKeyMapChange(AnimatorStateEventKeyMap.KeyChange keyChange)
        {
            //Try to remove old key from this
            RemoveKeyMap(keyChange.keyMap, keyChange.oldKey);

            //Try to add new key to this
            InjectKeyMap(keyChange.keyMap);
        }

        void RemoveKeyMap(AnimatorStateEventKeyMap keyMap, AnimatorStateBehaviourKey key)
        {
            //If key does not exist in dictionary, stop
            if (!KeysToMaps.ContainsKey(key))
                return;

            //Get keymap entry
            KeyEntry entry = TryGetKeyMapEntry(KeysToMaps[key], keyMap);

            RemoveKeyEntry(entry, key);
        }

        void RemoveKeyEntry(KeyEntry entry, AnimatorStateBehaviourKey key) 
        {
            //If the keymap isn't stored here, stop
            if (entry == null)
                return;

            //Remove from list and dispose of handle
            KeysToMaps[key].Remove(entry);
            entry.onKeyChangeHandle.Dispose();

            //If the dictionary entry that contained that keymap entry is now empty, remove it
            if (KeysToMaps[key].Count <= 0)
                KeysToMaps.Remove(key);
        }

        void RemoveNullEntryFromList(AnimatorStateBehaviourKey key, int index) 
        {
            //Cache current list
            List<KeyEntry> list = KeysToMaps[key];

            //Remove and check if dict entry has an empty list
            list.RemoveAt(index);
            if (list.Count <= 0)
                KeysToMaps.Remove(key);
        }

        /// <summary>
        /// Triggers all keymaps registered here for a given key. Returns true if at least one KeyMap was activated
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool TriggerAllKeyMapsForKey(AnimatorStateBehaviourKey key, AnimatorStateEventKeyMap.AnimatorStateBehaviourEventParameters parameters) 
        {
            //If key doesnt exist, stop 
            if (!KeysToMaps.ContainsKey(key))
                return false;

            //Cache current list
            List<KeyEntry> list = KeysToMaps[key];

            //If the list is empty
            if (list.Count <= 0)
                return false;

            //Call each
            for (int i = 0; i < list.Count; i++) 
            {
                if (list[i] == null)
                {
                    RemoveNullEntryFromList(key, i);
                    i--;
                }
                else 
                {
                    list[i].keyMap.onTrigger.Trigger(parameters);
                }
            }

            //Check again after the removal of null values
            if (list.Count <= 0)
                return false;
            else
                return true;
        }


    }
}


