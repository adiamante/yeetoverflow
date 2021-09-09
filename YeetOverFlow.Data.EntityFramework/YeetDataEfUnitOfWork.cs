using YeetOverFlow.Core;
using YeetOverFlow.Core.EntityFramework;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Data.EntityFramework
{
    public class YeetDataEfUnitOfWork : YeetEfUnitOfWork<YeetDataSet, YeetData>, IYeetUnitOfWork<YeetDataSet, YeetData>
    {
        public YeetDataEfUnitOfWork(YeetEfDbContext<YeetDataSet, YeetData> context,
            IRepository<YeetLibrary<YeetDataSet>> libraries,
            IRepository<YeetDataSet> lists,
            IRepository<YeetData> items,
            IRepository<YeetEvent<YeetData>> events) :
                base(context, libraries, lists, items, events)
        {
        }
    }
}
