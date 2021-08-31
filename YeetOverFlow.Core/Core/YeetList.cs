using System;
using System.Collections.Generic;

namespace YeetOverFlow.Core
{
    //<out T> : covariance allows specifying less specific type argument. Works if T is only used for return values
    public interface IYeetListBaseRead<out T> where T : YeetItem
    {
        IEnumerable<T> Children { get; }
        int Count { get; }
    }

    //<in T> : contravariance allows specifying more specific type argument. Works if T is only used for input parameters
    public interface IYeetListBaseWrite<in T> where T : YeetItem
    {
        void AddChild(T newChild);
        void InsertChildAt(int targetSequence, T newChild);
        void MoveChild(int targetSequence, T childToMove);
        void RemoveChild(T childToRemove);
        void RemoveChildAt(int targetSequence);
    }

    public interface IYeetListBase<T> : IYeetListBaseRead<T>, IYeetListBaseWrite<T> where T : YeetItem
    {
        T this[int key] { get; }
    }


    public abstract class YeetListBase<T> : YeetItem, IYeetListBase<T>
        where T : YeetItem
    {
        protected List<T> _children = new List<T>();

        public YeetListBase() : base()
        {

        }
        public YeetListBase(Guid guid) : base(guid)
        {

        }

        #region Indexer
        public T this[int index]
        {
            get => _children[index];
        }
        #endregion Indexer

        public void AddChild(T newChild)
        {
            InsertChildAt(_children.Count, newChild);
        }

        public void InsertChildAt(int targetSequence, T newChild)
        {
            if (newChild == null) throw new ArgumentNullException(nameof(newChild));
            if (_children.Contains(newChild)) throw new InvalidOperationException($"Cannot add '{nameof(newChild)}' because it already exists in _children.");

            //Make sure target sequence is not out of bounds
            if (targetSequence < 0)
            {
                targetSequence = 0;
            }
            else if (targetSequence > _children.Count)
            {
                targetSequence = _children.Count;
            }

            foreach (T child in _children)
            {
                if (child.Sequence >= targetSequence)
                {
                    SetSequence(child.Sequence + 1, child);
                }
            }

            _children.Insert(targetSequence, newChild);
            SetSequence(targetSequence, newChild);
        }

        public void RemoveChild(T childToRemove)
        {
            if (childToRemove == null) throw new ArgumentNullException(nameof(childToRemove));
            T storedChildToRemove = _children.Find(c => c.Guid == childToRemove.Guid);
            int targetSequence = storedChildToRemove.Sequence;

            foreach (T child in _children)
            {
                if (child.Sequence > targetSequence)
                {
                    SetSequence(child.Sequence - 1, child);
                }
            }

            _children.Remove(storedChildToRemove);
            SetSequence(targetSequence, storedChildToRemove);
        }

        public void MoveChild(int targetSequence, T childToMove)
        {
            if (!_children.Contains(childToMove)) throw new InvalidOperationException($"Cannot move '{nameof(childToMove)}' because it does not exists in _children.");
            int originalChildSequence = childToMove.Sequence;

            bool movingDown = targetSequence > originalChildSequence;
            foreach (T child in _children)
            {
                //if targeted child
                if (child.Sequence == originalChildSequence)
                {
                    SetSequence(targetSequence, child);
                }
                //if moving down and child is within the range
                else if (movingDown && child.Sequence <= targetSequence && child.Sequence > originalChildSequence)
                {
                    //child got bumped up
                    SetSequence(child.Sequence - 1, child);
                }
                //if moving up and child is within the range
                else if (!movingDown && child.Sequence >= targetSequence && child.Sequence < originalChildSequence)
                {
                    //child got bumped down
                    SetSequence(child.Sequence + 1, child);
                }
            }

            _children.Remove(childToMove);
            _children.Insert(targetSequence, childToMove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            T child = _children[targetSequence];
            RemoveChild(child);
        }

        public IEnumerable<T> Children
        {
            get { return _children; }
        }

        public int Count => _children.Count;
    }

    public class YeetList : YeetListBase<YeetItem>
    {
        public YeetList() : base()
        {

        }
        public YeetList(Guid guid) : base(guid)
        {

        }
    }

    public class YeetList<T> : YeetListBase<T>
        where T : YeetItem
    {
        public YeetList() : base()
        {

        }
        public YeetList(Guid guid) : base(guid)
        {

        }
        public override string Kind => nameof(YeetList);
    }
}
