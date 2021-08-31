using System;

namespace YeetOverFlow.Core.Application.Commands
{
    public class RemoveYeetItemCommand<TChild> : YeetCommand where TChild : YeetItem
    {
        public override YeetCommandKind Kind => YeetCommandKind.RemoveYeetItem;
        public Guid TargetListGuid { get; }
        public Guid TargetChildGuid { get; }
        public RemoveYeetItemCommand(Guid targetListGuid, Guid targetChildGuid)
        {
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            TargetChildGuid = targetChildGuid != Guid.Empty ? targetChildGuid : throw new ArgumentException(nameof(targetChildGuid));
        }
    }
}
