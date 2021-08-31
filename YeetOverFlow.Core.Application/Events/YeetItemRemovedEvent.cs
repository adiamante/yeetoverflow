using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Core.Application.Events
{
    public class YeetItemRemovedEvent<TChild> : YeetEvent<TChild> where TChild : YeetItem
    {
        public override YeetEventKind Kind => YeetEventKind.YeetItemRemoved;
        public Guid TargetListGuid { get; }
        public Guid TargetChildGuid { get; }
        public TChild TargetChild { get; }
        YeetItemRemovedEvent() //For Entity Framework
        {

        }
        public YeetItemRemovedEvent(Guid targetListGuid, Guid targetChildGuid, TChild targetChild)
        {
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            TargetChildGuid = targetChildGuid != Guid.Empty ? targetChildGuid : throw new ArgumentException(nameof(targetChildGuid));
            TargetChild = targetChild ?? throw new ArgumentNullException(nameof(targetChild));
        }

    }
}
