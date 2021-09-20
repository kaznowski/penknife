using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleDash.ComponentManagement.UIManagement
{
    public class CanvasTransition : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The folowing canvases will be on an activated state at the end of this transition.")]
        public List<CanvasGroupManager> activeCanvases;

        [Tooltip("The folowing canvases will be on an activated state at the end of this transition, but will not be interactable.")]
        public List<CanvasGroupManager> backgroundCanvases;

        [Tooltip("One of these elements will be selected at the end of this transition.")]
        public SelectableList firstSelectedElement;

        [Header("Settings")]
        [Tooltip("How long should this transition take in seconds.")]
        public float transitionTimeInSeconds = 1;

        /// <summary>
        /// This class represents a list of 'Selectable' elements from which one will be selected 
        /// </summary>
        [SerializeField]
        public class SelectableList
        {
            [Tooltip("List of elements from which one will be selected.")]
            [SerializeField] List<Selectable> _selectableElements = new List<Selectable>();

            [Tooltip("Selection criteria by which one of the listed elements will be selected.\n\n" +
                     "Sequential Priority: Elements from the top of the list have more priority than the elements with higher indexes on the list.\n\n" +
                     "Random: The chosen element is selected randomly.")]

            public SelectionCriteria defaultCriteria = SelectionCriteria.sequentialPriority;

            public List<Selectable> SelectableElements
            {
                get
                {
                    //If the list is null, initialize it
                    if (_selectableElements == null) _selectableElements = new List<Selectable>();
                    return _selectableElements;
                }
            }

            public enum SelectionCriteria
            {
                sequentialPriority,
                random
            }

            #region Public Variables

            /// <summary>
            /// Returns a selectable element based on the selection criteria
            /// </summary>
            /// <returns></returns>
            public Selectable Select()
            {
                switch (defaultCriteria)
                {
                    case (SelectionCriteria.sequentialPriority): return SelectBySequentialPriority();
                    case (SelectionCriteria.random): return SelectAtRandom();
                    default: return null;
                }
            }

            /// <summary>
            /// Selects an element from the list using the top elements as priority;
            /// </summary>
            /// <returns></returns>
            public Selectable SelectBySequentialPriority()
            {
                for (int i = 0; i < SelectableElements.Count; i++)
                {
                    //If element is null, remove from list and continue
                    if (SelectableElements[i] == null)
                    {
                        SelectableElements.RemoveAt(i);
                        i--;
                        continue;
                    }

                    //If the current selectable is interactable, return it
                    if (SelectableElements[i].IsInteractable()) return SelectableElements[i];
                }

                //If no interactable element was found, return null
                return null;
            }

            /// <summary>
            /// Randomly selects an element from the list, if that element is available.
            /// </summary>
            /// <returns></returns>
            public Selectable SelectAtRandom()
            {
                //Generate ordered List
                List<int> randomIndexes = ListExtension.GenerateRandomIndexList(SelectableElements.Count);

                //Check each index
                for (int i = 0; i < randomIndexes.Count; i++)
                    if (SelectableElements[randomIndexes[i]].IsInteractable())
                        return SelectableElements[randomIndexes[i]];

                //If no index was selectable, return null
                return null;
            }

            #endregion
        }
    }


}