namespace YeetOverFlow.Core
{
    public interface IYeetKeyedList<T> : IYeetListBase<T> where T : YeetItem, IKeyedItem
    {
        T this[string key] { get; set; }

        bool ContainsKey(string key);
        void Remove(string keyToRemove);
        void Init();
    }
}
