using System;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public class RemoveYeetItemCommandHandler<TParent, TChild> : YeetCommandHandler<RemoveYeetItemCommand<TChild>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public RemoveYeetItemCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore) : base(unitOfWork, eventStore)
        {

        }

        protected override Result Execute(RemoveYeetItemCommand<TChild> command)
        {
            Guid targetListGuid = command.TargetListGuid;
            Guid targetChildGuid = command.TargetChildGuid;
            TParent targetList = _unitOfWork.YeetLists.GetById(targetListGuid, l => l.Children);
            TChild targetChild = _unitOfWork.YeetItems.GetById(targetChildGuid);

            if (targetList == null) throw new InvalidOperationException($"Could not find targetList with provided '{nameof(targetListGuid)}'");
            if (targetChild == null) throw new InvalidOperationException($"Could not find targetChild with provided '{nameof(targetChildGuid)}'");

            int targetChildSequence = targetChild.Sequence;
            targetList.RemoveChild(targetChild);
            CascadeDelete(_unitOfWork, targetChild);

            foreach (TChild child in targetList.Children)
            {
                if (child.Sequence > targetChildSequence)
                {
                    _unitOfWork.YeetItems.Update(child);
                }
            }

            _eventStore.DispatchEvent(new YeetItemRemovedEvent<TChild>(targetListGuid, targetChildGuid, targetChild));

            if (!command.DeferCommit) _unitOfWork.Save();

            return Result.Ok();
        }

        private void CascadeDelete(IYeetUnitOfWork<TParent, TChild> unitOfWork, YeetItem item)
        {
            if (item is TParent list)
            {
                TParent loadedList = unitOfWork.YeetLists.GetById(item.Guid, l => l.Children);
                foreach (TChild child in loadedList.Children)
                {
                    if (child is TParent subList)
                    {
                        CascadeDelete(unitOfWork, subList);
                    }
                }
            }

            unitOfWork.YeetItems.Delete(item.Guid);
        }
    }
}
