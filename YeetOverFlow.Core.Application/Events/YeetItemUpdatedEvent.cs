using System;
using System.Collections.Generic;

namespace YeetOverFlow.Core.Application.Events
{
    public class YeetItemUpdatedEvent<TChild> : YeetEvent<TChild> where TChild : YeetItem
    {
        public override YeetEventKind Kind => YeetEventKind.YeetItemUpdated;
        public Guid TargetGuid { get; }
        public IDictionary<String, String> Updates { get; }
        public IDictionary<String, String> Original { get; }
        YeetItemUpdatedEvent() //For Entity Framework
        {

        }
        public YeetItemUpdatedEvent(Guid targetGuid, IDictionary<String, String> updates, IDictionary<String, String> original)
        {
            TargetGuid = targetGuid != Guid.Empty ? targetGuid : throw new ArgumentException(nameof(targetGuid));
            Updates = updates ?? throw new ArgumentNullException(nameof(updates));
            Original = original ?? throw new ArgumentNullException(nameof(original));
        }
    }
}
