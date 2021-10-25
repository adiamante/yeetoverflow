using System;
using System.Collections.Generic;

namespace YeetOverFlow.Core.Application.Commands
{
    public class UpdateYeetItemCommand<TChild> : YeetCommand where TChild : YeetItem
    {
        public override YeetCommandKind Kind => YeetCommandKind.UpdateYeetItem;
        public Guid TargetGuid { get; }
        public IDictionary<string, string> Updates { get; }
        public UpdateYeetItemCommand(Guid targetGuid, IDictionary<string, string> updates, bool deferCommit = false)
        {
            TargetGuid = targetGuid != Guid.Empty ? targetGuid : throw new ArgumentException(nameof(targetGuid));
            Updates = updates ?? throw new ArgumentNullException(nameof(updates));
            DeferCommit = deferCommit;
        }
    }
}
