using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Reflection;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public class UpdateYeetItemCommandHandler<TParent, TChild> : YeetCommandHandler<UpdateYeetItemCommand<TChild>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public UpdateYeetItemCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore) : base(unitOfWork, eventStore)
        {

        }

        protected override Result Execute(UpdateYeetItemCommand<TChild> command)
        {
            Guid targetGuid = command.TargetGuid;
            IDictionary<string, string> updates = command.Updates;
            Dictionary<string, string> original = new Dictionary<string, string>();
            TChild targetItem = _unitOfWork.YeetItems.GetById(targetGuid);

            if (targetItem == null) throw new InvalidOperationException($"Could not find targetItem with provided '{nameof(targetGuid)}'");

            foreach (KeyValuePair<string, string> kvp in updates)
            {
                PropertyInfo propInfo = ReflectionHelper.PropertyInfoCollection[targetItem.GetType()][kvp.Key];
                if (propInfo != null)
                {
                    TypeConverter converter = ReflectionHelper.TypeConverterCache[propInfo.PropertyType];
                    var value = converter.ConvertFromString(kvp.Value);
                    
                    var originalValue = propInfo.GetValue(targetItem);
                    original.Add(kvp.Key, originalValue?.ToString());

                    propInfo.SetValue(targetItem, value);
                }
            }

            _eventStore.DispatchEvent(new YeetItemUpdatedEvent<TChild>(targetGuid, updates, original));

            if (!command.DeferCommit) _unitOfWork.Save();

            return Result.Ok();
        }
    }
}
