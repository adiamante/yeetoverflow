using System;

namespace YeetOverFlow.Core.Application.Events
{
    //public abstract class YeetEvent
    //{
    //    public Guid Guid { get; set; }
    //}


    public abstract class YeetEvent<TChild> : IYeetEvent<TChild> where TChild : YeetItem
    {
        public Guid Guid { get; set; }
        //public Guid LibraryGuid { get; }  //don't see a need yet
        public abstract YeetEventKind Kind { get; }
    }
}
