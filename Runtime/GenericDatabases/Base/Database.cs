using DoubleDash.CodingTools.ClassExtensions;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDash.CodingTools.GenericDatabases {

    [System.Serializable]
    public class Database<TypeClass> : IDatabaseReader<TypeClass>, IDatabaseModifier<TypeClass>
        where TypeClass : class, INameable // Objects within the database must have a name to index them by.
    {
        #region Variables

        [SerializeField, Tooltip("Name of this Database. This is used to index this Database within Database groups.")]
        string _name;

        [SerializeField, Tooltip("References to the values and/or references that will be contained in this database.\n\n" +
                                 "On play, these entries will be removed from this list and indexed by their names into a dictionary for instant access.\n\n"+
                                 "If you wish to inject more entries, you can add them into this list via the inspector, and during the next time the dictionary is used, that entry will be added into the dictionary.")]

        VariableReference<TypeClass>[] injectedEntries;

        //Dictionary that will contain the entries added for O(1) access.
        protected Dictionary<string, TypeClass> _dictionary; 

        #endregion

        #region Properties

        public virtual string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Get the database's dictionary, initializing it first, if necessary.
        /// </summary>
        protected Dictionary<string, TypeClass> Dictionary 
        {
            get 
            {
                //Initialize the dictionary if needed 
                if (_dictionary == null) _dictionary = new Dictionary<string, TypeClass>();

                //If there are injected entries to be added, add them to the dictionary
                if (injectedEntries != null) TransferInjectedEntriesToDictionary();

                //Return the dictionary
                return _dictionary;
            }
        }

        #endregion

        #region Public Functions

        public Dictionary<string, TypeClass> CloneDatabase() => Dictionary.Clone();

        #region IDatabaseReader Functions

        public virtual bool HasEntry(string entryName) => Dictionary.ContainsKey(entryName);

        /// <summary>
        /// Searches the database for an entry of a given name. Returns null if the entry is not found.
        /// </summary>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public TypeClass GetEntry(string entryName)
        {
            if (HasEntry(entryName)) return GetEntry(entryName);
            else                     return null;
        }

        public TypeClass[] GetAllEntriesAsArray()
        {
            return GetAllEntriesAsList().ToArray();
        }

        public List<TypeClass> GetAllEntriesAsList()
        {
            List<TypeClass> returnList = new List<TypeClass>();

            foreach (KeyValuePair<string, TypeClass> entry in Dictionary) 
            {
                returnList.Add(entry.Value);
            }

            return returnList;
        }

        #endregion

        #region IDatabaseModifier Functions

        public void AddEntry(TypeClass entry) 
        {
            Dictionary.Add(entry.Name, entry);
        }

        public void RemoveEntry(TypeClass entry)
        {
            Dictionary.Remove(entry.Name);
        }

        public void ClearDatabase()
        {
            Dictionary.Clear();
        }

        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Passes all array values to the dictionary, cleaning the array's memory afterwards.
        /// </summary>
        void TransferInjectedEntriesToDictionary() 
        {
            //Stop if the serialized array of entries is null.
            if (injectedEntries == null) return;
            
            //Iterate through array to get entries, identifying null entries and adding the non-null ones.
            foreach (VariableReference<TypeClass> injectedEntry in injectedEntries) 
            {
                //If injected entry is null, stop
                if (injectedEntry == null) 
                {
                    Debug.LogError("Null injected entry on database '" + this.Name + "'.");
                    continue;
                }
                else if (injectedEntry.Value == null) //If the injected entry has null value
                {
                    Debug.LogError("Injected entry on database '" + this.Name + "' has a null Value.");
                    continue;
                }
                //Get value from reference
                if (injectedEntry != null) _dictionary.Add(injectedEntry.Value.Name, injectedEntry.Value);
            }
            //Clear the array's data.
            injectedEntries = null;
        }

        #endregion
    }
}


