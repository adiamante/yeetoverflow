using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetObservableKeyedList<TChild> : YeetItemViewModelBaseExtended, IYeetKeyedList<TChild>, INotifyCollectionChanged
        where TChild : YeetItem, IKeyedItem
    {
        protected YeetKeyedList<TChild> _yeetKeyedList = new YeetKeyedList<TChild>();
        protected ObservableCollection<TChild> _children = new ObservableCollection<TChild>();

        public YeetObservableKeyedList() : this(Guid.NewGuid())
        {

        }

        public YeetObservableKeyedList(Guid guid) : base(guid)
        {
            _children.CollectionChanged += _children_CollectionChanged;
        }

        private void _children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        #region Indexer
        public TChild this[int index] { get => _yeetKeyedList[index]; }
        public TChild this[string key]
        {
            get { return _yeetKeyedList[key]; }
            set
            {
                bool newChild = !_yeetKeyedList.ContainsKey(key);
                _yeetKeyedList[key] = value;
                if (newChild)
                {
                    _children.Add(value);
                }
            }
        }
        #endregion Indexer

        public IEnumerable<TChild> Children => _children;

        public int Count => ((IYeetListBaseRead<TChild>)_yeetKeyedList).Count;

        public void SetInvalidChildCallback(Action<string, TChild> invalidChildCallback)
        {
            _yeetKeyedList.SetInvalidChildCallback(invalidChildCallback);
        }

        #region INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs changes)
        {
            CollectionChanged?.Invoke(this, changes);
        }
        #endregion INotifyCollectionChanged

        public void AddChild(TChild newChild)
        {
            InsertChildAt(_yeetKeyedList.Count, newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<TChild>)_yeetKeyedList).ContainsKey(key);
        }

        public virtual void Init()
        {
            _yeetKeyedList.Init();

            if (_children.Count > 0 && _yeetKeyedList.Count == 0)
            {
                foreach (var child in _children)
                {
                    _yeetKeyedList.InsertChildAt(child.Sequence, child);
                }
            }
        }

        public void InsertChildAt(int targetSequence, TChild newChild)
        {
            ((IYeetListBaseWrite<TChild>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
            _children.Insert(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, TChild childToMove)
        {
            var originalSequence = childToMove.Sequence;
            ((IYeetListBaseWrite<TChild>)_yeetKeyedList).MoveChild(targetSequence, childToMove);

            foreach (var child in _yeetKeyedList.Children)
            {
                SetSequence(child.Sequence, child);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, new List<TChild>() { childToMove }, targetSequence, originalSequence));
        }

        public void Remove(string keyToRemove)
        {
            TChild childToRemove = _yeetKeyedList[keyToRemove];
            RemoveChild(childToRemove);
        }
        public void RemoveChildAt(int targetSequence)
        {
            TChild childToRemove = _yeetKeyedList[targetSequence];
            RemoveChild(childToRemove);
        }

        public void RemoveChild(TChild childToRemove)
        {
            ((IYeetListBaseWrite<TChild>)_yeetKeyedList).RemoveChild(childToRemove);
            _children.Remove(childToRemove);
        }
    }
}
