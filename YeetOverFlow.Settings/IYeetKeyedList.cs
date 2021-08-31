using YeetOverFlow.Core;

namespace YeetOverFlow.Settings
{
    public interface IYeetKeyedList<T> : IYeetListBase<T> where T : YeetItem, IKeyedItem
    {
        T this[string key] { get; set; }

        string Key { get; }

        bool ContainsKey(string key);
        void Remove(string keyToRemove);
        void Init();
    }
}
