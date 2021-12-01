using System;
using System.Collections.Generic;

namespace YeetOverFlow.Core
{
    public class YeetKeyedList<TParent, TChild> : YeetKeyedList<TChild>
        where TChild : YeetItem, IKeyedItem
        where TParent : TChild
    {
        public YeetKeyedList() : this(Guid.NewGuid())
        {
        }

        public YeetKeyedList(Guid guid) : base(guid)
        {
        }

        #region Indexer
        public override TChild this[string key]
        {
            get
            {
                if (!_dict.ContainsKey(key))
                {
                    var child = (TParent)Activator.CreateInstance(typeof(TParent), Guid.NewGuid(), key);
                    AddChild(child);
                    Validate(key, child);
                }
                return _dict[key];
            }
            set
            {
                if (!_dict.ContainsKey(key))
                {
                    _yeetList.AddChild(value);
                    Validate(key, value);
                }
                _dict[key] = value;
            }
        }
        #endregion Indexer
    }

    public class YeetKeyedList<TChild> : YeetItem, IYeetKeyedList<TChild> 
        where TChild : YeetItem, IKeyedItem
    {
        private Action<String, TChild> _invalidChildCallback;
        protected YeetList<TChild> _yeetList;

        public IEnumerable<TChild> Children => ((IYeetListBaseRead<TChild>)_yeetList).Children;

        public int Count => ((IYeetListBaseRead<TChild>)_yeetList).Count;

        protected Dictionary<string, TChild> _dict = new Dictionary<string, TChild>();
        public YeetKeyedList() : this(Guid.NewGuid())
        {
        }

        public YeetKeyedList(Guid guid) : base(guid)
        {
            _yeetList = new YeetList<TChild>();
        }

        public void AddChild(TChild newChild)
        {
            InsertChildAt(_yeetList.Count, newChild);
        }

        public void InsertChildAt(int targetSequence, TChild newChild)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).InsertChildAt(targetSequence, newChild);
            _dict.Add(newChild.Key, newChild);
        }

        public void MoveChild(int targetSequence, TChild childToMove)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(TChild childToRemove)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).RemoveChild(childToRemove);
            _dict.Remove(childToRemove.Key);
        }
        public void RemoveChildAt(int targetSequence)
        {
            TChild childToRemove = _yeetList[targetSequence];
            RemoveChild(childToRemove);
        }
        public void Remove(string keyToRemove)
        {
            TChild childToRemove = _dict[keyToRemove];
            RemoveChild(childToRemove);
        }

        #region Indexer
        public TChild this[int index]
        {
            get => _yeetList[index];
        }

        public virtual TChild this[string key]
        {
            get 
            {   
                if (!_dict.ContainsKey(key))
                {
                    return default(TChild);
                }
                return _dict[key]; 
            }
            set
            {
                if (!_dict.ContainsKey(key))
                {
                    _yeetList.AddChild(value);
                    Validate(key, value);
                }
                _dict[key] = value;
            }
        }
        #endregion Indexer

        #region Methods
        public Boolean ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public void SetInvalidChildCallback(Action<string, TChild> invalidChildCallback)
        {
            _invalidChildCallback = invalidChildCallback;
        }

        protected void Validate(string key, TChild child)
        {
            if (String.IsNullOrEmpty(child.Key))
            {
                _invalidChildCallback?.Invoke(key, child);
            }
            if (String.IsNullOrEmpty(child.Name))
            {
                child.Name = key;
            }
        }

        //sync list with dictionary
        public void Init()
        {
            if (_yeetList.Count > 0)
            {
                _dict.Clear();
                foreach (var itm in _yeetList.Children)
                {
                    _dict.Add(itm.Key, itm);
                }
            }
        }
        #endregion Methods
    }
}
