using System;

namespace YeetOverFlow.Core
{
    //Aggregate Root
    public class YeetLibrary<TRootType> : YeetLibraryBase
    {
        protected TRootType _root;
        public YeetLibrary() : base()
        {

        }
        public YeetLibrary(Guid guid) : base(guid)
        {

        }
        public virtual TRootType Root { get => _root; set => _root = value; }
    }

    public class YeetLibraryBase : AggregateRoot
    {
        public string Owner { get; set; }
        public YeetLibraryBase() : base()
        {

        }
        public YeetLibraryBase(Guid guid) : base(guid)
        {

        }
    }
}
