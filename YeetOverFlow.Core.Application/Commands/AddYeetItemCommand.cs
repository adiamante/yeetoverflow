using System;

namespace YeetOverFlow.Core.Application.Commands
{
    public class AddYeetItemCommand<TChild> : YeetCommand
    {
        public override YeetCommandKind Kind => YeetCommandKind.AddYeetItem;
        public Guid TargetListGuid { get; }
        public TChild Child { get; }
        public int TargetChildSequence { get; }
        public AddYeetItemCommand(Guid targetListGuid, TChild child, int targetChildSequence)
        {
            TargetListGuid = targetListGuid != Guid.Empty ? targetListGuid : throw new ArgumentException(nameof(targetListGuid));
            Child = child ?? throw new ArgumentNullException(nameof(child));
            TargetChildSequence = targetChildSequence;
        }
    }
}
