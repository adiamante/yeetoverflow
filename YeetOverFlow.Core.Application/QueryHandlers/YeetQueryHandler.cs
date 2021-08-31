using System;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Queries;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.Application.QueryHandlers
{
    public abstract class YeetQueryHandler<TParameter, TResultValue, TParent, TChild> : IQueryHandler<TParameter, Result<TResultValue>>
        where TParameter : IQuery
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        protected IYeetUnitOfWork<TParent, TChild> _unitOfWork;
        public YeetQueryHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Result<TResultValue> Handle(TParameter query)
        {
            try
            {
                return Retrieve(query);
            }
            catch (Exception ex)
            {
                return Result.Fail<TResultValue>(ex);
            }
        }

        protected abstract Result<TResultValue> Retrieve(TParameter query);
    }
}
