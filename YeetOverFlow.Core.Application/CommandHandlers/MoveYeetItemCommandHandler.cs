using System;
using System.Linq;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public class MoveYeetItemCommandHandler<TParent, TChild> : YeetCommandHandler<MoveYeetItemCommand<TChild>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public MoveYeetItemCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore) : base(unitOfWork, eventStore)
        {

        }

        protected override Result Execute(MoveYeetItemCommand<TChild> command)
        {
            Guid targetListGuid = command.TargetListGuid;
            Guid targetChildGuid = command.TargetChildGuid;
            Guid originalListGuid = command.OriginalListGuid;
            TParent targetList = _unitOfWork.YeetLists.GetById(targetListGuid, l => l.Children);
            TChild targetChild = _unitOfWork.YeetItems.GetById(targetChildGuid);
            
            if (targetList == null) throw new InvalidOperationException($"Could not find targetList with provided '{nameof(targetListGuid)}'");
            if (targetChild == null) throw new InvalidOperationException($"Could not find targetChild with provided '{nameof(targetChildGuid)}'");

            int originalChildSequence = targetChild.Sequence,
                targetChildSequence = command.TargetChildSequence;

            //If targetChild is a list and targetList is a descendant of targetChild, throw exception
            //(TChild)(object) casting is questionable. A fix would be a type constraint that TParent is also type of TChild
            if (targetChild is TParent targetChildList && IsDescendant(targetChildList, (TChild)(object)targetList))
            {
                throw new InvalidOperationException("Cannot move a list within own heirarchy.");
            }

            //Move within the same parent list
            if (originalListGuid == Guid.Empty || originalListGuid == targetListGuid)     
            {
                if (!targetList.Children.Any(child => child.Guid == targetChild.Guid))
                {
                    throw new InvalidOperationException("When exluding originalListGuid, targetList should already contain targetChild.");
                }

                int minSequence = Math.Min(targetChildSequence, originalChildSequence);
                int maxSequence = Math.Min(targetChildSequence, originalChildSequence);

                targetList.MoveChild(targetChildSequence, targetChild);
                foreach (TChild child in targetList.Children)
                {
                    if (child.Sequence >= minSequence && child.Sequence <= maxSequence)
                    {
                        _unitOfWork.YeetItems.Update(child);
                    }
                }
            }
            else //Moving item to new parent list
            {
                TParent originalList = _unitOfWork.YeetLists.GetById(originalListGuid, l => l.Children);

                if (originalList == null) throw new InvalidOperationException($"Could not find targetList with provided '{nameof(originalListGuid)}'");

                if (!originalList.Children.Any(child => child.Guid == targetChild.Guid))
                {
                    throw new InvalidOperationException("Original list does not already contain targetChild.");
                }

                //Remove from original list
                originalList.RemoveChild(targetChild);

                //Update original list children
                foreach (TChild child in originalList.Children)
                {
                    if (child.Sequence > originalChildSequence)
                    {
                        _unitOfWork.YeetItems.Update(child);
                    }
                }

                //Add to target list
                targetList.InsertChildAt(targetChildSequence, targetChild);

                //Update target list children
                foreach (TChild child in targetList.Children)
                {
                    if (child.Sequence >= targetChildSequence)
                    {
                        _unitOfWork.YeetItems.Update(child);
                    }
                }

                _unitOfWork.YeetItems.Update(targetChild);
            }

            _eventStore.DispatchEvent(
                new YeetItemMovedEvent<TChild>(originalListGuid, targetListGuid, targetChildGuid, targetChildSequence));

            if (!command.DeferCommit) _unitOfWork.Save();

            return Result.Ok();
        }

        public bool IsDescendant(TParent list, TChild item)
        {
            foreach (TChild child in list.Children)
            {
                //Might want to handle equality in YeetItem class
                if (child.Guid == item.Guid)
                {
                    return true;
                }

                if (child is TParent childList)
                {
                    if (IsDescendant(childList, item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
