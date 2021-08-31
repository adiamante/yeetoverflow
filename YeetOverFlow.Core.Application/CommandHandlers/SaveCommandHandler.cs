using System;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public class SaveCommandHandler<TParent, TChild> : YeetCommandHandler<SaveCommand<TChild>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public SaveCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore) : base(unitOfWork, eventStore)
        {

        }

        protected override Result Execute(SaveCommand<TChild> command)
        {
            _unitOfWork.Save();
            return Result.Ok();
        }
    }
}
