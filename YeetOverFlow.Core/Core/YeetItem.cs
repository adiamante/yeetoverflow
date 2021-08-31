using System;

namespace YeetOverFlow.Core
{
    public class YeetItem : Entity
    {
        protected Guid _guid;
        protected int _sequence;
        protected string _name;

        public override Guid Guid => _guid;
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public virtual Int32 Sequence => _sequence;
        public virtual string Kind => nameof(YeetItem);
        public YeetItem() : this(Guid.NewGuid())
        {

        }

        public YeetItem(Guid guid) : base(guid)
        {
            _guid = guid;
        }
        protected virtual void SetSequence(int sequence, YeetItem yeetItem)
        {
            yeetItem._sequence = sequence;
        }
    }
}
