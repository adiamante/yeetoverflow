using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.EntityFramework
{
    public class YeetEfUnitOfWork<TParent, TChild> : IYeetUnitOfWork<TParent, TChild>
        where TParent : YeetItem, IYeetListBase<TChild>
        where TChild : YeetItem
    {
        YeetEfDbContext<TParent, TChild> _context;

        public IRepository<YeetLibrary<TParent>> YeetLibraries { get; private set; }
        public IRepository<TParent> YeetLists { get; private set; }
        public IRepository<TChild> YeetItems { get; private set; }
        public IRepository<YeetEvent<TChild>> Events { get; private set; }

        public YeetEfUnitOfWork(YeetEfDbContext<TParent, TChild> context,
            IRepository<YeetLibrary<TParent>> libraries, 
            IRepository<TParent> lists, 
            IRepository<TChild> items,
            IRepository<YeetEvent<TChild>> events)
        {
            _context = context;
            YeetLibraries = libraries;
            YeetLists = lists;
            YeetLists = lists;
            YeetItems = items;
            Events = events;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
