using System;

namespace YeetOverFlow.Core.Application.Events
{
    public class YeetItemMovedEvent<TChild> : YeetEvent<TChild> where TChild : YeetItem
    {
        public override YeetEventKind Kind => YeetEventKind.YeetItemMoved;
        public Guid OriginalListGuid { get; }
        public Guid TargetListGuid { get; }
        public Guid TargetChildGuid { get; }
        public int TargetChildSequence { get; }
        YeetItemMovedEvent() //For Entity Framework
        {

        }
        public YeetItemMovedEvent(Guid originalListGuid, Guid targetListGuid, Guid targetChildGuid, int targetChildSequence)
        {
            OriginalListGuid = originalListGuid != Guid.Empty ? originalListGuid : throw new ArgumentException(nameof(originalListGuid));
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            TargetChildGuid = targetChildGuid != Guid.Empty ? targetChildGuid : throw new ArgumentException(nameof(targetChildGuid));
            TargetChildSequence = targetChildSequence;
        }
    }
}
