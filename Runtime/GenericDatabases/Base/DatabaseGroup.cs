using System.Collections.Generic;

namespace DoubleDash.CodingTools.GenericDatabases
{
    [System.Serializable]
    public abstract class DatabaseGroup<T> : Database<IDatabaseReader<T>>, IDatabaseReader<T>
        where T : class, INameable //Objects within the database must have a name to index them by.
    {
        /// <summary>
        /// Checks if an entry exists on the database's dictionary.
        /// </summary>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public new bool HasEntry(string entryName)
        {
            if (GetEntry(entryName) != null) return true;
            else                             return false;
        }

        /// <summary>
        /// Gets an entry of a given name. Or returns null if that entry isn't on the dictionary.
        /// </summary>
        /// <returns></returns>
        public new T GetEntry(string entryName)
        {
            //Store keys that have become null and will be removed
            List<string> nullKeys = new List<string>();

            //Result to be returned
            T result = null;

            //Iterate through all entries
            foreach (KeyValuePair<string, IDatabaseReader<T>> pair in Dictionary) 
            {
                //Check if entry is null. If it is, add to list of keys that will be removed.
                if (pair.Value == null)
                {
                    nullKeys.Add(pair.Key);
                }
                else 
                {
                    //Search that database for the entry.
                    T searchResult = pair.Value.GetEntry(entryName);

                    //If it was found, stop searching
                    if (searchResult != null) 
                    {
                        result = searchResult;
                        break;
                    }
                }
            }

            //Clear all null entries from dictionary
            while (nullKeys.Count > 0) 
            {
                //Remove that key from the dictionary and from the list of null keys
                Dictionary.Remove(nullKeys[0]);
                nullKeys.RemoveAt(0);
            }

            //If it got here, then the entry isn't on any database registered on this
            return null;
        }

        /// <summary>
        /// Returns an array of all entries from the database.
        /// </summary>
        /// <returns></returns>
        T[] IDatabaseReader<T>.GetAllEntriesAsArray()
        {
            return (this as IDatabaseReader<T>).GetAllEntriesAsList().ToArray();
        }

        /// <summary>
        /// Returns a list of all entries from the database.
        /// </summary>
        /// <returns></returns>
        List<T> IDatabaseReader<T>.GetAllEntriesAsList()
        {
            //All entries will be concatenated into the same list
            List<T> returnList = new List<T>();

            //For each database within this collection
            foreach (KeyValuePair<string, IDatabaseReader<T>> entry in _dictionary)
            {
                //Append to the returnlist all entries found in that database
                returnList.AddRange(entry.Value.GetAllEntriesAsList());
            }

            //Returns the appended collection
            return returnList;
        }

    }
}