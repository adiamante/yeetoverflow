using System;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Commands;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.CommandHandlers
{
    public abstract class YeetCommandHandler<TParameter, TParent, TChild> : ICommandHandler<TParameter, Result>
        where TParameter : ICommand
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        protected IYeetUnitOfWork<TParent, TChild> _unitOfWork;
        protected IYeetEventStore<YeetEvent<TChild>, TChild> _eventStore;
        public YeetCommandHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork, IYeetEventStore<YeetEvent<TChild>, TChild> eventStore)
        {
            _unitOfWork = unitOfWork;
            _eventStore = eventStore;
        }
        public Result Handle(TParameter query)
        {
            try
            {
                return Execute(query);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex);
            }
        }

        protected abstract Result Execute(TParameter query);
    }
}
