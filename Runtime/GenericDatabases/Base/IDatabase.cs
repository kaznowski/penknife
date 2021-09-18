namespace DoubleDash.CodingTools.GenericDatabases
{
    public interface IDatabaseReader<T> : INameable
        where T : class, INameable //Objects within the database must have a name by which to index them by.
    {
        public bool HasEntry(string entryName);

        public T GetEntry(string entryName);

        public T[] GetAllEntriesAsArray();

        public System.Collections.Generic.List<T> GetAllEntriesAsList();
    }

    public interface IDatabaseModifier<T> : INameable
        where T : class, INameable //Objects within the database must have a name by which to index them by.
    {
        public void AddEntry(T entry);

        public void RemoveEntry(T entry);

        public void ClearDatabase();
    }
}