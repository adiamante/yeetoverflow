using System;
using YeetOverFlow.Core.Application.Data.Queries;

namespace YeetOverFlow.Core.Application.Queries
{
    public class GetYeetLibraryByGuidQuery : Query
    {
        public Guid Guid { get; }

        public GetYeetLibraryByGuidQuery(Guid guid)
        {
            Guid = guid != Guid.Empty ? guid : throw new ArgumentException(nameof(guid));
        }
    }
}
