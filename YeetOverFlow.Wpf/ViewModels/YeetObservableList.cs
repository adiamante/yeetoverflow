using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetObservableList<TChild> : YeetItemViewModelBaseExtended, IYeetListBase<TChild>, INotifyCollectionChanged
        where TChild : YeetItem
    {
        protected YeetList<TChild> _yeetList = new YeetList<TChild>();
        protected ObservableCollection<TChild> _children = new ObservableCollection<TChild>();

        public IEnumerable<TChild> Children => _children;
        public TChild this[int index] { get => _yeetList[index]; }
        public int Count => ((IYeetListBaseRead<TChild>)_yeetList).Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)_children).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)_children).CollectionChanged -= value;
            }
        }

        public void AddChild(TChild newChild)
        {
            InsertChildAt(_yeetList.Count, newChild);
        }

        public YeetObservableList() : this(Guid.NewGuid())
        {

        }

        public YeetObservableList(Guid guid) : base(guid)
        {
        }

        public virtual void Init()
        {
            if (_children.Count > 0 && _yeetList.Count == 0)
            {
                foreach (var child in _children)
                {
                    _yeetList.AddChild(child);
                }
            }
        }

        public void InsertChildAt(int targetSequence, TChild newChild)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).InsertChildAt(targetSequence, newChild);
            _children.Add(newChild);
        }

        public void MoveChild(int targetSequence, TChild childToMove)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).MoveChild(targetSequence, childToMove);
            _children.Remove(childToMove);
            _children.Insert(targetSequence, childToMove);
        }

        public void RemoveChild(TChild childToRemove)
        {
            ((IYeetListBaseWrite<TChild>)_yeetList).RemoveChild(childToRemove);
            _children.Remove(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            TChild childToRemove = _yeetList[targetSequence];
            RemoveChild(childToRemove);
        }
    }
}
