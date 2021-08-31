using System;

namespace YeetOverFlow.Core.Application.Commands
{
    public class MoveYeetItemCommand<TChild> : YeetCommand where TChild : YeetItem
    {
        public override YeetCommandKind Kind => YeetCommandKind.MoveYeetItem;
        public Guid OriginalListGuid { get; }
        public Guid TargetListGuid { get; }
        public Guid TargetChildGuid { get; }
        public int TargetChildSequence { get; }
        public MoveYeetItemCommand(Guid originalListGuid, Guid targetListGuid, Guid targetChildGuid, int targetChildSequence)
        {
            //OriginalListGuid = originalListGuid != Guid.Empty ? originalListGuid : throw new ArgumentException(nameof(originalListGuid));
            OriginalListGuid = originalListGuid;    //when empty, item is being moved from the same list
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            TargetChildGuid = targetChildGuid != Guid.Empty ? targetChildGuid : throw new ArgumentException(nameof(targetChildGuid));
            TargetChildSequence = targetChildSequence;
        }
    }
}
