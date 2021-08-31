using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Queries;

namespace YeetOverFlow.Core.Application.QueryHandlers
{
    public class GetYeetLibraryByGuidQueryHandler<TParent, TChild> : YeetQueryHandler<GetYeetLibraryByGuidQuery, YeetLibrary<TParent>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public GetYeetLibraryByGuidQueryHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork) : base(unitOfWork)
        {

        }

        protected override Result<YeetLibrary<TParent>> Retrieve(GetYeetLibraryByGuidQuery request)
        {
            YeetLibrary<TParent> library = _unitOfWork.YeetLibraries.GetById(request.Guid, includePropertyExpression : lib => lib.Root);
            _unitOfWork.YeetLists.RecursiveLoadCollection(library.Root, root => root.Children);

            return Result.Ok(library);
        }
    }
}
