using System;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public class AddYeetItemCommandHandler<TParent, TChild> : YeetCommandHandler<AddYeetItemCommand<TChild>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public AddYeetItemCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore) : base(unitOfWork, eventStore)
        {

        }

        protected override Result Execute(AddYeetItemCommand<TChild> command)
        {
            Guid targetListGuid = command.TargetListGuid;
            TParent targetList = _unitOfWork.YeetLists.GetById(targetListGuid, l => l.Children);
            TChild targetChild = command.Child;
            Int32 targetChildSequence = command.TargetChildSequence;

            if (targetList == null) throw new InvalidOperationException($"Could not find targetList with provided '{nameof(targetListGuid)}'");

            //Add to target list
            if (_unitOfWork.YeetItems.GetById(targetChild.Guid) == null)
            {
                _unitOfWork.YeetItems.Insert(targetChild);
            }

            targetList.InsertChildAt(targetChildSequence, targetChild);
            if (!command.DeferCommit) _unitOfWork.Save();

            //Update target list children
            foreach (TChild child in targetList.Children)
            {
                if (child.Sequence >= targetChildSequence)
                {
                    _unitOfWork.YeetItems.Update(child);
                }
            }

            _eventStore.DispatchEvent(
                new YeetItemAddedEvent<TChild>(targetListGuid, targetChild, targetChildSequence));
            
            if (!command.DeferCommit) _unitOfWork.Save();

            return Result.Ok();
        }
    }
}
