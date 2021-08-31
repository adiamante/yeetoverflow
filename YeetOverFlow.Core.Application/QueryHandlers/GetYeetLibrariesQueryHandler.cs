using System.Collections.Generic;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Queries;

namespace YeetOverFlow.Core.Application.QueryHandlers
{
    public class GetYeetLibrariesQueryHandler<TParent, TChild> : YeetQueryHandler<GetYeetLibrariesQuery, IEnumerable<YeetLibrary<TParent>>, TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        public GetYeetLibrariesQueryHandler(IYeetUnitOfWork<TParent, TChild> unitOfWork) : base(unitOfWork)
        {

        }


        protected override Result<IEnumerable<YeetLibrary<TParent>>> Retrieve(GetYeetLibrariesQuery request)
        {
            IEnumerable<YeetLibrary<TParent>> libraries = _unitOfWork.YeetLibraries.Get(includePropertyExpression : lib => lib.Root);
            foreach (YeetLibrary<TParent> library in libraries)
            {
                _unitOfWork.YeetLists.RecursiveLoadCollection(library.Root, root => root.Children);
            }
 
            return Result.Ok(libraries);
        }
    }
}
