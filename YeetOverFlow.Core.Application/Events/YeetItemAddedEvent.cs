using System;

namespace YeetOverFlow.Core.Application.Events
{
    public class YeetItemAddedEvent<TChild> : YeetEvent<TChild> where TChild : YeetItem
    {
        public override YeetEventKind Kind => YeetEventKind.YeetItemAdded;
        public Guid TargetListGuid { get; }
        public TChild Child { get; }
        public int TargetChildSequence { get; }
        YeetItemAddedEvent() //For Entity Framework
        {

        }
        public YeetItemAddedEvent(Guid targetListGuid, TChild child, int targetChildSequence)
        {
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            Child = child ?? throw new ArgumentNullException(nameof(child));
            TargetChildSequence = targetChildSequence;
        }
    }
}
